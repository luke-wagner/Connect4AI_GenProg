partial class C4SmartAI : Connect4 {
    /////////////////////////////////////////////////
    // PRIVATE METHODS
    /////////////////////////////////////////////////

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