public class C4AITrainer : C4SmartAI {
    public const int TUNE_MAX = 1000;
    public const int TUNE_MIN = -1000;
    public bool playingAsPlayer { get; private set;}

    public bool won { get => this.checkForWin(myNumber); }
    public bool boardFull { get => this.checkFull(); }
    public (int x, int y) LastMove { get => lastMove; }
    public (int x, int y) LastMoveCoords { get => getGridCoordinates(lastMove.x, lastMove.y); }

    public SortedDictionary<int, int> TuningValues { get => this.tuningValues; }

    public C4AITrainer(bool playAsPlayer){
        playingAsPlayer = playAsPlayer;
        if (playingAsPlayer){
            myNumber = 1;
        } else {
            myNumber = 2;
        }
    }

    public C4AITrainer(bool playAsPlayer, SortedDictionary<int, int> tuning){
        playingAsPlayer = playAsPlayer;
        if (playingAsPlayer){
            myNumber = 1;
        } else {
            myNumber = 2;
        }

        tuningValues = tuning;
    }

    public override string ToString()
    {
        string s = "AI Trainer Stats:\n";
        s += "My number: " + myNumber + "\n";
        s += "{ Borders friendly neighbor: " + tuningValues[1] +  " }\n";
        s += "{ borders more than one friendly neighbor: " + tuningValues[2] +  " }\n";
        s += "{ borders opponent neighbor: " + tuningValues[3] +  " }\n";
        s += "{ borders more than one opponent neighbor: " + tuningValues[4] +  " }\n";
        s += "{ continues run of two: " + tuningValues[5] +  " }\n";
        s += "{ continues run of three or more: " + tuningValues[6] +  " }\n";
        s += "{ stops run of three: " + tuningValues[7] +  " }\n";
        s += "{ stops run of four or more: " + tuningValues[8] +  " }\n";
        return s;
    }

    public override void Play()
    {
        Console.WriteLine("//////////////////////////////////////////////////////////////");
        Console.WriteLine("//// WARNING: PLAYING AITRAINER, NOT INSTANCE OF C4SMARTAI");
        Console.WriteLine("//// - MAY NOT BEHAVE AS EXPECTED.");
        Console.WriteLine("//////////////////////////////////////////////////////////////");
        Console.WriteLine("\n-- PROCEED? (Y/N)");
        string input = Console.ReadLine();
        Console.WriteLine();
        if (input == "Y"){
            base.Play();
        } else {
            return;
        }
    }

    public void randomlyTune(){
        var keys = new List<int>(tuningValues.Keys);
        foreach (var key in keys){
            int randInt = rand.Next() % (TUNE_MAX - TUNE_MIN) + 1;
            randInt -= TUNE_MAX;
            tuningValues[key] = randInt;
        }
    }

    public int Move(){
        int move = getComputerMove();
        playToCol(move, playingAsPlayer);
        return move;
    }

    public void OpponentMove(int move){
        // validate move
        if (colIsFull(move))
        {
            return;
        }
        playToCol(move, !playingAsPlayer);
    }
}