partial class C4SmartAI : Connect4 {
    protected int myNumber; // the number that represents this computer's token on the grid (1 or 2)

    // Each key represents an attribute that either belongs or does not belong to a possible move (see description
    // of each attribute below)
    // All tuning values are assigned a number between 0 and 1000;
    // I used genetic programming to tune these values, see "EX_03/extra/README.md" for more on how I accomplished this
    protected SortedDictionary<int, int> tuningValues = new SortedDictionary<int, int>(){
        { 1, -198 }, // borders friendly neighbor
        { 2, -23 }, // borders more than one friendly neighbor
        { 3, 414 }, // borders opponent neighbor
        { 4, 393 }, // borders more than one opponent neighbor
        { 5, 343 }, // continues run of two
        { 6, 978 }, // continues run of three or more
        { 7, 373 }, // stops run of three
        { 8, 972 }, // stops run of four or more
    };

    public C4SmartAI(){
        startMessage = "Hmm.. Doesn't this look familiar. Let's see if you can beat Connect 4 with my new and improved AI! Bwahaha!\n";
        myNumber = 2;
    }

    /*
    Overrides Connect4.getComputerMove(). Assigns a score to each possible move and chooses the move with
    the best score
    */
    protected override int getComputerMove() {
        int[] moveScores = new int[NUM_COLS]; // the evaluated merit of each move based on the attributes defined above

        // assign move values
        for (int i = 1; i <= NUM_COLS; i++){
            if (colIsFull(i)){
                // invalid move
                moveScores[i - 1] = int.MinValue;
                continue;
            } else {
                // NOTE: Private methods are implemented in extra/C4SmartAIPriv.cs
                (int x, int y) dropSpot = findDropSpot(i); // where token would land
                int friendlyNeighbors = numFriendlyNeighbors(dropSpot.x, dropSpot.y);
                int opponentNeighbors = numOpponentNeighbors(dropSpot.x, dropSpot.y);
                int lengthMyRun = continuesRun(dropSpot.x, dropSpot.y);
                int lengthOpponentRun = stopsRun(dropSpot.x, dropSpot.y);

                // for each tuning value, if attribute is satisfied, add it to the move's score
                if (friendlyNeighbors > 1){
                    moveScores[i - 1] += tuningValues[2];
                } else if (friendlyNeighbors == 1){
                    moveScores[i - 1] += tuningValues[1];
                }
                if (opponentNeighbors > 1){
                    moveScores[i - 1] += tuningValues[4];
                } else if (opponentNeighbors == 1){
                    moveScores[i - 1] += tuningValues[3];
                }
                if (lengthMyRun >= 4) {
                    moveScores[i - 1] += tuningValues[6];
                } else if (lengthMyRun == 3){
                    moveScores[i - 1] += tuningValues[5];
                }
                if (lengthOpponentRun >= 4) {
                    moveScores[i - 1] += tuningValues[8];
                } else if (lengthOpponentRun == 3){
                    moveScores[i - 1] += tuningValues[7];
                }
            }
        }

        // choose best move
        int bestMove = 0;
        int bestMoveValue = int.MinValue;
        for (int i = 1; i <= NUM_COLS; i++){
            if (moveScores[i - 1] > bestMoveValue){
                bestMoveValue = moveScores[i - 1];
                bestMove = i;
            }
        }

        return bestMove;
    }
}