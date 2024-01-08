using System.Text.Json;

class AIDueler : IPlayable{
    private Random rand;
    private double runTime;
    public double RunTime {get; private set;}

    void SlowPlay(ref C4AITrainer AI_1, ref C4AITrainer AI_2){
            while (true){
                int AI_1_move = AI_1.Move();
                AI_2.OpponentMove(AI_1_move);
                AI_1.DisplayGrid(false, true, true);
                if (AI_1.won){
                    Console.WriteLine("AI 1 won!");
                    return;
                }
                Thread.Sleep(800);

                if (AI_1.boardFull){
                    Console.WriteLine("Draw!");
                    return;
                }

                int AI_2_move = AI_2.Move();
                AI_1.OpponentMove(AI_2_move);
                AI_2.DisplayGrid(false, true, true);
                if (AI_2.won){
                    Console.WriteLine("AI 2 won!");
                    return;
                }
                Thread.Sleep(800);

                if (AI_1.boardFull){
                    Console.WriteLine("Draw!");
                    return;
                }
            }
        }

    C4AITrainer? FastPlay(C4AITrainer AI_1, C4AITrainer AI_2){
            while (true){
                int AI_1_move = AI_1.Move();
                AI_2.OpponentMove(AI_1_move);
                if (AI_1.won){
                    return AI_1;
                }

                if (AI_1.boardFull){
                    return null;
                }

                int AI_2_move = AI_2.Move();
                AI_1.OpponentMove(AI_2_move);
                if (AI_2.won){
                    return AI_2;
                }

                if (AI_1.boardFull){
                    return null;
                }
            }
    }
    
    public SortedDictionary<int, int> findNewWinner(){
        SortedDictionary<int, int>? winnerTuning = null;

        for(int i = 0; i < DuelerConfig.NUM_TRIALS_FOR_NEW_WINNER; i++){
            C4AITrainer winner;
            do {
                C4AITrainer AI_1;
                if (winnerTuning == null){
                    AI_1 = new C4AITrainer();
                    AI_1.playingAsPlayer = true;
                    AI_1.initGame();
                    AI_1.randomlyTune();
                } else {
                    AI_1 = new C4AITrainer(winnerTuning);
                    AI_1.playingAsPlayer = true;
                    AI_1.initGame();
                }
                C4AITrainer AI_2 = new C4AITrainer();
                AI_2.playingAsPlayer = false;
                AI_2.initGame();
                AI_2.randomlyTune();

                winner = FastPlay(AI_1, AI_2);
            } while (winner == null);

            winnerTuning = winner.TuningValues;
        }

        return winnerTuning;
    }

    public void addWinnersToPool(){
        string inputFile = "../../../extra/res/randWinners";
        string jsonString;
        try {
            jsonString = File.ReadAllText(inputFile);
        } catch {
            jsonString = "";
        }

        List<SortedDictionary<int, int>> winnersData;
        if (jsonString != ""){
            winnersData = JsonSerializer.Deserialize<List<SortedDictionary<int, int>>>(jsonString);
            // Output json data
            string json = JsonSerializer.Serialize(winnersData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("../../../extra/res/reducedPool.json", json);
        } else {
            winnersData = new List<SortedDictionary<int, int>>();
            Console.WriteLine("ERROR: No winners to add to pool");
        }
    }

    public void generateRandomWinners(){
        // Input stored winners into winners data
        List<SortedDictionary<int, int>> winnersData = new List<SortedDictionary<int, int>>();
        for (int i = 0; i < DuelerConfig.NUM_WINNERS_TO_GENERATE; i++){
            SortedDictionary<int, int> newWinner = findNewWinner();    
            winnersData.Add(newWinner);
        }

        // Output json data
        string json = JsonSerializer.Serialize(winnersData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("../../../extra/res/randWinners", json);
    }

    public void repopulatePool(){
        string inputFile = "../../../extra/res/reducedPool.json";
        string jsonString;
        try {
            jsonString = File.ReadAllText(inputFile);
        } catch {
            jsonString = "";
        }

        List<SortedDictionary<int, int>> reducedPool;
        if (jsonString != ""){
            reducedPool = JsonSerializer.Deserialize<List<SortedDictionary<int, int>>>(jsonString);
        } else {
            Console.WriteLine("ERROR: Nothing in pool to repopulate");
            return;
        }

        List<SortedDictionary<int, int>> newPool = new List<SortedDictionary<int, int>>();

        foreach (var organism in reducedPool){
            newPool.Add(organism);
            for (int i = 0; i < DuelerConfig.NUM_CHILDREN_PER_WINNER; i++){
                int mateNumber;
                SortedDictionary<int, int> mate;
                do {
                    mateNumber = rand.Next() % reducedPool.Count();
                    mate = reducedPool[mateNumber];
                } while (mate == organism);

                SortedDictionary<int, int> child = new SortedDictionary<int, int>();
                for (int j = 1; j <= organism.Count(); j++){
                    int keepGene = rand.Next() % 2;
                    if (keepGene == 0){
                        // don't keep gene
                        child[j] = mate[j];
                    } else {
                        // do keep gene
                        child[j] = organism[j];
                    }
                }
                newPool.Add(child);
            }
        }

        // Output json data
        string json = JsonSerializer.Serialize(newPool, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("../../../extra/res/pool.json", json);
    }

    private bool _dividePool(ref List<SortedDictionary<int, int>>? pool, string outputFile){
        if (pool.Count() < 2){
            Console.WriteLine("ERROR: Cannot reduce pool further");
            return false;
        }

        List<SortedDictionary<int, int>> newPool = new List<SortedDictionary<int, int>>();
        List<SortedDictionary<int, int>> alreadyFought = new List<SortedDictionary<int, int>>();
        List<SortedDictionary<int, int>> leftInPool = new List<SortedDictionary<int, int>>(pool);
        foreach (var organism in pool){
            if (!leftInPool.Contains(organism)){
                continue;
            }

            int opponentNumber;
            SortedDictionary<int, int> opponent;
            do {
                opponentNumber = rand.Next() % leftInPool.Count();
                opponent = leftInPool[opponentNumber];
                opponentNumber = pool.IndexOf(opponent);
            } while(opponent == organism);

            C4AITrainer AI_1 = new C4AITrainer(organism);
            C4AITrainer AI_2 = new C4AITrainer(opponent);
            AI_1.playingAsPlayer = true;
            AI_2.playingAsPlayer = false;
            AI_1.initGame();
            AI_2.initGame();

            C4AITrainer? winner = FastPlay(AI_1, AI_2);
            if (winner == null){
                winner = AI_1;
            }
            
            leftInPool.Remove(organism);
            leftInPool.Remove(opponent);
            newPool.Add(winner.TuningValues);
        }

        // Output json data
        string json = JsonSerializer.Serialize(newPool, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputFile, json);

        pool = newPool;

        return true;
    }

    public bool dividePool(ref List<SortedDictionary<int, int>>? pool, string outputFile="../../../extra/res/reducedPool.json"){
        return _dividePool(ref pool, outputFile);
    }

    public bool dividePool(string inputFile, string outputFile="../../../extra/res/reducedPool.json"){
        List<SortedDictionary<int, int>> pool = new List<SortedDictionary<int, int>>();
        string jsonString;
        try {
            jsonString = File.ReadAllText(inputFile);
        } catch {
            jsonString = "";
        }
        if (jsonString != ""){
            pool = JsonSerializer.Deserialize<List<SortedDictionary<int, int>>>(jsonString);
        } else {
            Console.WriteLine("ERROR: No organisms found in pool");
            return false;
        }

        return _dividePool(ref pool, outputFile);   
    }

    public bool reducePool(){
        string inputFile = "../../../extra/res/pool.json";
        string jsonString;
        try {
            jsonString = File.ReadAllText(inputFile);
        } catch {
            jsonString = "";
        }
        List<SortedDictionary<int, int>> pool;
        if (jsonString != ""){
            pool = JsonSerializer.Deserialize<List<SortedDictionary<int, int>>>(jsonString);
            if (pool.Count() / Math.Pow(2, DuelerConfig.NUM_DIVISIONS_PER_REDUCTION) < DuelerConfig.NUM_WINNERS_TO_GENERATE){
                Console.WriteLine("Warning: Reducing pool further will reduce size past original population. Proceed? (Y/N)");
                string input = Console.ReadLine();
                if (input != "Y"){
                    return false;
                }
            }
        } else {
            Console.WriteLine("ERROR: No organisms found in pool");
            return false;
        }

        for (int i = 0; i < DuelerConfig.NUM_DIVISIONS_PER_REDUCTION; i++){
            dividePool(ref pool);
        }

        return true;
    }

    public void Play()
    {
        var stopWatch = System.Diagnostics.Stopwatch.StartNew();
        rand = new Random();

        //generateRandomWinners();
        addWinnersToPool();        

        // repopulate pool with children of winners some number of times
        for (int i = 0; i < DuelerConfig.NUM_CYCLES; i++){
            var start = DateTime.Now;
            repopulatePool();
            reducePool();
            var end = DateTime.Now;
            Console.WriteLine("Iteration {0}: {1}", i + 1, end - start);
        }

        string reducedPoolContents = File.ReadAllText("../../../extra/res/reducedPool.json");
        File.WriteAllText("../../../extra/res/finalTuning.json", reducedPoolContents);
        while(dividePool(inputFile:"../../../extra/res/finalTuning.json", outputFile:"../../../extra/res/finalTuning.json"));

        stopWatch.Stop();
        RunTime = stopWatch.ElapsedMilliseconds / 1000;
    }

    public void Quit()
    {
        
    }
}