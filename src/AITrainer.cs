class C4AITrainer : C4SmartAI {
    private Random rand;
    public const int TUNE_MAX = 1000;
    public const int TUNE_MIN = -1000;
    public bool playingAsPlayer;

    public bool won { get => this.checkForWin(myNumber); }
    public bool boardFull { get => this.checkFull(); }

    public C4AITrainer(){

    }

    public C4AITrainer(SortedDictionary<int, int> tuning){
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
        s += "{ continues run of three: " + tuningValues[6] +  " }\n";
        s += "{ stops run of two: " + tuningValues[7] +  " }\n";
        s += "{ stops run of three: " + tuningValues[8] +  " }\n";
        return s;
    }

    public SortedDictionary<int, int> TuningValues { get => this.tuningValues; }

    public void initGame(){
        if (playingAsPlayer){
            myNumber = 1;
        } else {
            myNumber = 2;
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
        playToCol(move, !playingAsPlayer);
    }
}