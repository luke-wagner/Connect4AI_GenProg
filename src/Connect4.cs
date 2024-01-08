class Connect4 : IPlayable
{
    protected const int NUM_ROWS = 6;
    protected const int NUM_COLS = 7;

    protected string startMessage = "Welcome to Connect 4!\n";
    protected int [,] grid; // 0 = unmarked, 1 = blue, 2 = red
    protected Random rand;
    private (int x, int y) lastMove;

    protected void Start (){
        rand = new Random();
        grid = new int [6,7];
        lastMove = (0, 0);
    }

    public virtual void Play() {
        Start();
        Console.WriteLine(startMessage);
        DisplayGrid();

        while (true) {
            int playerMove = getPlayerMove();
            playToCol(playerMove);
            Console.Beep(500, 500);

            if (checkForWin(1)){
                DisplayGrid(showLastMove:false);
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("You won!");
                Console.ResetColor();
                Console.WriteLine();
                Quit();
                return;
            }

            if (!checkFull()){
                int computerMove = getComputerMove();
                playToCol(computerMove, false);

                if (checkForWin(2)){
                    DisplayGrid();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("You lost :(");
                    Console.ResetColor();
                    Console.WriteLine();
                    Quit();
                    return;
                }
            }

            DisplayGrid();
            
            if (checkFull()) {
                // necessary to check if full again for the case where the grid is not full until after
                // the player moves
                Console.WriteLine("Lame....");
                Quit();
                return;
            }
        }
    }

    public void Quit() {
        Console.WriteLine("Thanks for playing!");
    }

    protected bool checkForWin(int numToCheck){
        // Only directions needed: right, up, diagonalRight, and diagonalLeft. Other directions not needed.
        // Down from the current cell would be detected by up from somewhere else
        // --------------------------------------------------------------------

        bool checkRight(int i, int j){
            int counter = 0;

            while (counter < 4 && j <= NUM_COLS){
                (int x, int y) = getGridCoordinates(i, j);
                if (grid[x, y] != numToCheck){
                    break;
                } else {
                    counter++;
                    j++;
                }
            }

            if (counter >= 4){
                return true;
            } else {
                return false;
            }
        }
        
        bool checkUp(int i, int j){
            int counter = 0;

            while (counter < 4 && i <= NUM_ROWS){
                (int x, int y) = getGridCoordinates(i, j);
                if (grid[x, y] != numToCheck){
                    break;
                } else {
                    counter++;
                    i++;
                }
            }

            if (counter >= 4){
                return true;
            } else {
                return false;
            }
        }

        // up and to the left
        bool checkDiagonalLeft(int i, int j){
            int counter = 0;

            while (counter < 4 && i <= NUM_ROWS && j >= 1){
                (int x, int y) = getGridCoordinates(i, j);
                if (grid[x, y] != numToCheck){
                    break;
                } else {
                    counter++;
                    i++;
                    j--;
                }
            }

            if (counter >= 4){
                return true;
            } else {
                return false;
            }
        }

        // up and to the right
        bool checkDiagonalRight(int i, int j){
            int counter = 0;

            while (counter < 4 && i <= NUM_ROWS && j <= NUM_COLS){
                (int x, int y) = getGridCoordinates(i, j);
                if (grid[x, y] != numToCheck){
                    break;
                } else {
                    counter++;
                    i++;
                    j++;
                }
            }

            if (counter >= 4){
                return true;
            } else {
                return false;
            }
        }

        // Check for each grid cell
        for (int i = 1; i <= NUM_ROWS; i++){
            for (int j = 1; j <= NUM_COLS; j++){
                // don't bother checking those that aren't the number we're looking for
                (int x, int y) = getGridCoordinates(i, j);
                if (grid[x, y] != numToCheck){
                    continue;
                }

                if (checkRight(i, j) || checkUp(i, j) || checkDiagonalLeft(i, j) || checkDiagonalRight(i, j)){
                    return true;
                }
            }
        }

        return false;
    }

    protected bool checkFull(){
        for (int i = 1; i <= NUM_COLS; i++){
            if (!colIsFull(i)){
                return false;
            }
        }
        return true;
    }

    protected (int x, int y) findDropSpot(int col){
        int i;
        for (i = 1; i <= NUM_COLS; i++){
            (int x, int y) coordToCheck = getGridCoordinates(i, col);
            if (grid[coordToCheck.x, coordToCheck.y] == 0){
                break;
            } else {
                continue;
            }
        }
        return (i, col);
    }

    protected void playToCol(int col, bool playerMove=true){
        (int x, int y) dropSpot = findDropSpot(col);
        markGridCell(dropSpot.x, dropSpot.y, playerMove);
        lastMove = (dropSpot.x, dropSpot.y);
    }

    protected bool colIsFull(int col){
        (int x, int y) spotToCheck = getGridCoordinates(NUM_ROWS, col);
        if (grid[spotToCheck.x, spotToCheck.y] != 0){
            return true;
        } else {
            return false;
        }
    }

    private int getPlayerMove(){
        Console.WriteLine("Which column would you like to place your token in?");
        int move;
        bool isInt = int.TryParse(Console.ReadLine(), out move);
        while (move < 1 || move > NUM_COLS || colIsFull(move) || !isInt){
            Console.WriteLine("Not a valid move. Try again");
            isInt = int.TryParse(Console.ReadLine(), out move);
        }

        return move;
    }

    protected virtual int getComputerMove(){
        int move;
        do {
            move = rand.Next() % NUM_COLS + 1;
        } while (colIsFull(move));

        return move;
    }

    // Converts grid coordinates as the user sees displayed to how they are actually stored in memory [i,j]
    // @return (int, int) - (-1, -1) if invalid set of coordinates
    protected (int row, int col) getGridCoordinates (int x, int y) {
        if (x > NUM_ROWS || x < 1 || y > NUM_COLS || y < 1){
            return (-1, -1);
        }

        int row = NUM_ROWS - x;
        int col = y - 1;

        // data validation

        return (row, col);
    }

    private void markGridCell(int x, int y, bool markBlue){
        (int row, int col) = getGridCoordinates(x, y);

        if (grid[row, col] == 0){
            if (markBlue){
                grid[row, col] = 1;
            } else {
                grid[row, col] = 2;
            }
        }
    }

    private void printGridLine(bool showCoordinates){
        if (showCoordinates){
            Console.Write("--");
        }
        for (int i = 0; i < NUM_COLS; i++){
                Console.Write("---");
            }
            Console.WriteLine();
    }

    private void printGridCoordinatesX(){
        Console.Write("  ");
        for (int i = 1; i <= NUM_COLS; i++){
            Console.Write(" {0} ", i);
        }
        Console.WriteLine();
    }

    public void DisplayGrid(bool showCoordinates=true, bool showLastMove=true, bool debugCharacters=false){
        if (showCoordinates) {
            printGridCoordinatesX();
        }

        printGridLine(showCoordinates);
        for (int i = 0; i < NUM_ROWS; i++){
            if (showCoordinates){
                Console.Write("{0} ", NUM_ROWS - i);
            }

            for (int j = 0; j < NUM_COLS; j++){
                Console.Write("|");

                if (grid[i, j] == 1){
                    Console.ForegroundColor = ConsoleColor.Blue;
                } else if (grid[i, j] == 2){
                    Console.ForegroundColor = ConsoleColor.Red;
                } 
                if ((i, j) == getGridCoordinates(lastMove.x, lastMove.y) && showLastMove == true){
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                } 

                if (debugCharacters){
                    if (grid[i, j] == 1){
                        Console.Write("0");
                    } else if (grid[i, j] == 2){
                        Console.Write("X");
                    } else {
                        Console.Write("O");
                    }
                } else {
                    Console.Write("O");
                }   
                
                Console.ResetColor();
                Console.Write("|");
            }
            Console.WriteLine();
        }
        printGridLine(showCoordinates);
        Console.WriteLine();
    }
}