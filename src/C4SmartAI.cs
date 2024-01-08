/*
File: C4SmartAI.cs
Author: Luke Wagner
Instructor: Dr. Matthew Bell
Section: CS-371-1
Description: Smart(er) AI for my Connect 4 game. I wanted to challenge myself to create an AI that doesn't
just random moves, but can look at the board and evaluate each move. It's worth noting it is still possible to
beat this AI (because it doesn't look ahead multiple moves).
*/
class C4SmartAI : Connect4 {
    protected int myNumber; // the number that represents this computer's token on the grid (1 or 2)

    // Each key represents an attribute that either belongs or does not belong to a possible move (see description
    // of each attribute below)
    // All tuning values are assigned a number between 0 and 1000;
    // I used genetic programming to tune these values. See README in "extra" folder for more explanation
    // on how I accomplished this.
    protected SortedDictionary<int, int> tuningValues = new SortedDictionary<int, int>(){
        { 1, 87 }, // borders friendly neighbor
        { 2, 398 }, // borders more than one friendly neighbor
        { 3, 197 }, // borders opponent neighbor
        { 4, -587 }, // borders more than one opponent neighbor
        { 5, 940 }, // continues run of two
        { 6, 880 }, // continues run of three
        { 7, 513 }, // stops run of two
        { 8, 978 }, // stops run of three
    };

    public C4SmartAI(){
        startMessage = "Hmm.. Doesn't this look familiar. Let's see if you can beat Connect 4 with my new and improved AI! Bwahaha!\n";
    }

    protected override int getComputerMove() {
        int[] moveScores = new int[NUM_COLS]; // the evaluated merit of each move based on the attributes defined above

        // assign move values
        for (int i = 1; i <= NUM_COLS; i++){
            if (colIsFull(i)){
                // invalid move
                moveScores[i - 1] = int.MinValue;
                continue;
            } else {
                (int x, int y) dropSpot = findDropSpot(i); // where token would land
                int friendlyNeighbors = numFriendlyNeighbors(dropSpot.x, dropSpot.y);
                int opponentNeighbors = numOpponentNeighbors(dropSpot.x, dropSpot.y);
                int lengthMyRun = continuesRun(dropSpot.x, dropSpot.y);
                int lengthOpponentRun = stopsRun(dropSpot.x, dropSpot.y);

                // for each tuning value, if attribute is satisfied, add it to the move's score
                if (friendlyNeighbors >= 1){
                    moveScores[i - 1] += tuningValues[1];
                }
                if (friendlyNeighbors > 1){
                    moveScores[i - 1] += tuningValues[2];
                }
                if (opponentNeighbors >= 1){
                    moveScores[i - 1] += tuningValues[3];
                }
                if (opponentNeighbors > 1){
                    moveScores[i - 1] += tuningValues[4];
                }
                if (lengthMyRun >= 3) {
                    moveScores[i - 1] += tuningValues[5];
                }
                if (lengthMyRun >= 4) {
                    moveScores[i - 1] += tuningValues[6];
                }
                if (lengthOpponentRun >= 3) {
                    moveScores[i - 1] += tuningValues[7];
                }
                if (lengthOpponentRun >= 4) {
                    moveScores[i - 1] += tuningValues[8];
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

    // @return int - the number of "friendly" tokens that are directly or diagonally adjacent to the space at (x, y)
    private int numFriendlyNeighbors (int x, int y){
        int counter = 0;

        bool checkCoord((int _x, int _y) coordToCheck){
            if (coordToCheck != (-1, -1) && grid[coordToCheck._x, coordToCheck._y] == myNumber){
                return true;
            } else {
                return false;
            }
        }

        // check for friendly neighbors in every direction of (x, y)
        (int _x, int _y) coordToCheck = getGridCoordinates(x + 1, y);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x + 1, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x + 1, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;

        return counter;
    }

    // @return int - the number of opponent's tokens that are directly or diagonally adjacent to the space at (x, y)
    private int numOpponentNeighbors (int x, int y){
        int counter = 0;

        bool checkCoord((int _x, int _y) coordToCheck){
            if (coordToCheck != (-1, -1) && grid[coordToCheck._x, coordToCheck._y] != myNumber && 
            grid[coordToCheck._x, coordToCheck._y] != 0){
                return true;
            } else {
                return false;
            }
        }

        // check for friendly neighbors in every direction of (x, y)
        (int _x, int _y) coordToCheck = getGridCoordinates(x + 1, y);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x + 1, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x + 1, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y + 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;
        coordToCheck = getGridCoordinates(x - 1, y - 1);
        counter = checkCoord(coordToCheck) ? counter + 1 : counter;

        return counter;
    }

    // @return int - the length of the longest run that would be formed by adding a token at (x, y)
    private int continuesRun (int x, int y){
        int run1 = 1; // up-down
        int run2 = 1; // left-right
        int run3 = 1; // diagonal-up-left/down-right
        int run4 = 1; // diagonal-up-right/down-left

        void checkForRun(int x_increment, int y_increment){
            int length = 1;
            (int _x, int _y) currentCoord = (x + x_increment, y + y_increment);
            (int _x, int _y) coordToCheck = getGridCoordinates(currentCoord._x, currentCoord._y);

            while (coordToCheck != (-1, -1) && grid[coordToCheck._x, coordToCheck._y] == myNumber){
                length++;
                currentCoord = (currentCoord._x + x_increment, currentCoord._y + y_increment);
                coordToCheck = getGridCoordinates(currentCoord._x, currentCoord._y);
            }

            if (y_increment == 0){
                run1 += length - 1;
            } else if (x_increment == 0){
                run2 += length - 1;
            } else if ((x_increment == 1 && y_increment == -1) || (x_increment == -1 && y_increment == 1)){
                run3 += length - 1;
            } else {
                run4 += length - 1;
            }
        }

        int x_increment = 1;
        int y_increment = 0;
        checkForRun(x_increment, y_increment);
        x_increment = 1;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = 1;
        y_increment = -1;
        checkForRun(x_increment, y_increment);
        x_increment = 0;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = 0;
        y_increment = -1;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = 0;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = -1;
        checkForRun(x_increment, y_increment);

        return Math.Max(Math.Max(run1, run2), Math.Max(run3, run4));
    }

    // @return int - the length of the longest opponent run that would be stopped by adding a token at (x, y)
    private int stopsRun (int x, int y){
        int run1 = 1; // up-down
        int run2 = 1; // left-right
        int run3 = 1; // diagonal-up-left/down-right
        int run4 = 1; // diagonal-up-right/down-left

        void checkForRun(int x_increment, int y_increment){
            int length = 1;
            (int _x, int _y) currentCoord = (x + x_increment, y + y_increment);
            (int _x, int _y) coordToCheck = getGridCoordinates(currentCoord._x, currentCoord._y);

            while (coordToCheck != (-1, -1) && grid[coordToCheck._x, coordToCheck._y] != myNumber && 
            grid[coordToCheck._x, coordToCheck._y] != 0){
                length++;
                currentCoord = (currentCoord._x + x_increment, currentCoord._y + y_increment);
                coordToCheck = getGridCoordinates(currentCoord._x, currentCoord._y);
            }

            if (y_increment == 0){
                run1 += length - 1;
            } else if (x_increment == 0){
                run2 += length - 1;
            } else if ((x_increment == 1 && y_increment == -1) || (x_increment == -1 && y_increment == 1)){
                run3 += length - 1;
            } else {
                run4 += length - 1;
            }
        }

        int x_increment = 1;
        int y_increment = 0;
        checkForRun(x_increment, y_increment);
        x_increment = 1;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = 1;
        y_increment = -1;
        checkForRun(x_increment, y_increment);
        x_increment = 0;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = 0;
        y_increment = -1;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = 0;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = 1;
        checkForRun(x_increment, y_increment);
        x_increment = -1;
        y_increment = -1;
        checkForRun(x_increment, y_increment);

        return Math.Max(Math.Max(run1, run2), Math.Max(run3, run4));
    }
}