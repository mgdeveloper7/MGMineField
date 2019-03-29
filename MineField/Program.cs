using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGMineCraft
{
    class MGMineCraft
    {

        enum MineStates
        {
            Mined,
            Hidden,
            Visible,
            Defused,
        };
        
        const int boardWidth = 10;
        const int boardHeight = 10;

        static int currentPlayerXPosition = 0;
        static int currentPlayerYPosition = 0;

        static int numberOfMinesDefused = 0;
        static bool endOfGame = false;

        static int numberOfMoves = 0;
        static int livesRemaining = 3;

        static MineStates[,] minefield = new MineStates[boardHeight, boardWidth];

        // *** Methods

        static void clearBoard()
        {
            for (int xPos = 0; xPos < boardHeight; xPos++)
            {
                for (int yPos = 0; yPos < boardWidth; yPos++)
                {
                    minefield[xPos, yPos] = MineStates.Hidden;
                }
            }
        }

        static void dropMinesRandomly()
        {
            // Put mines at random co-ordinates
            for (int i = 0; i < 15; i++)
            {
                int randomY = 0;
                int randomX = 0;

                Random random = new Random();
                randomY = random.Next(1, 10);
                randomX = random.Next(1, 10); 

                minefield[randomY, randomX] = MineStates.Mined;
            }
        }

        static void drawOnGrid(String item, Boolean isMineAtPosition)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (isMineAtPosition)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write("{0,2}",item);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void displayBoard() //This will display the board,and will also change the colour of the board being displayed.
        {
            {
                Console.Clear();
                Console.WriteLine("MG's Minefield");
                Console.Write("Current Position: {0}{1}       ", (char)(currentPlayerYPosition + 65), currentPlayerXPosition);  // Show as C3, B2 etc
                Console.Write("Moves taken: {0}        ", numberOfMoves);
                Console.WriteLine("Lives remaining: {0}", livesRemaining);

                for (int xPos = 0; xPos < boardWidth; xPos = xPos + 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0,2}", xPos);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine("\n");
            for (int yPos = 1; yPos < boardHeight; yPos = yPos + 1)
            {
                for (int xPos = 0; xPos < boardWidth; xPos = xPos + 1)
                {
                    if (xPos == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("{0,2}", (char)(yPos+64));  // Convert column down to A, B, C etc
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        // Print grid layout.  Place a different character and colour depending on whether a mine found or not
                        MineStates mineState = minefield[yPos, xPos];
                        switch (mineState)
                        {
                            case MineStates.Mined:
                                drawOnGrid("-", false);
                                break;
                            case MineStates.Hidden:
                                drawOnGrid("-", false);
                                break;
                            case MineStates.Visible:
                                drawOnGrid(numberOfMinesAroundPos(yPos, xPos).ToString(), false);
                                break;
                            case MineStates.Defused:
                                drawOnGrid("*", true);
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }

        // Calculates the mines around the space on the grid where the player has moved to
        // Display later to give them a clue 
        static int numberOfMinesAroundPos(int l, int m)
        {
            int surroundingMines = 0;
            for (int yPos = l - 1; yPos < l + 2; yPos++) {
                for (int xPos = m - 1; xPos < m + 2; xPos++) {
                    if (yPos != 0 && xPos != 0 && yPos < boardHeight && xPos < boardWidth) {
                        if (minefield[yPos, xPos] == MineStates.Mined || minefield[yPos, xPos] == MineStates.Defused) {
                            surroundingMines++;
                        }
                    }
                }
            }
            return surroundingMines;
        }

        static void initialisePlayer()
        {
            currentPlayerXPosition = 5;
            currentPlayerYPosition = 1;
            minefield[currentPlayerYPosition, currentPlayerXPosition] = MineStates.Visible;
        }

        static void movePlayer()
        {

            // Assume player is avoiding mines on moving
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("D = Down      U = Up      L = Left     R = Right      ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Move: ");
            String direction = (Console.ReadLine());

            bool playerHasMoved = false;

            // Ensure we stay in the boards bounds 
            if (direction.ToUpper() == "D")
            {
                if (currentPlayerYPosition == boardHeight-1)
                {
                    endOfGame = true;
                }

                else if (currentPlayerYPosition < (boardHeight-1) )
                {
                    currentPlayerYPosition++;
                    numberOfMoves++;
                    playerHasMoved = true;
                }
            }
            else if (direction.ToUpper() == "U")
            {
                if (currentPlayerYPosition > 1)
                {
                    currentPlayerYPosition--;
                    numberOfMoves++;
                    playerHasMoved = true;
                }
            }
            else if (direction.ToUpper() == "L")
            {
                if (currentPlayerXPosition > 1)  
                {
                    currentPlayerXPosition--;
                    numberOfMoves++;
                    playerHasMoved = true;
                }
            }
            else if (direction.ToUpper() == "R")
            {
                if (currentPlayerXPosition < (boardWidth-1) )
                {
                    currentPlayerXPosition++;
                    numberOfMoves++;
                    playerHasMoved = true;
                }
            }

            if (playerHasMoved)
            {
                if (minefield[currentPlayerYPosition, currentPlayerXPosition] == MineStates.Mined)
                {
                    livesRemaining--;
                    // Show player where mine was (at last position)

                    minefield[currentPlayerYPosition, currentPlayerXPosition] = MineStates.Defused;
                    if (livesRemaining == 0)
                    {
                        endOfGame = true;
                    }
                }
                else if (minefield[currentPlayerYPosition, currentPlayerXPosition] == MineStates.Hidden)
                {
                    // Continue unless player has got to the end of the board
                    minefield[currentPlayerYPosition, currentPlayerXPosition] = MineStates.Visible; 

                    if (currentPlayerYPosition == boardHeight)
                    {
                        endOfGame = true;
                    }
                }
            }

        }
 
         // *************************************************************************

        static void Main()
        {

            endOfGame = false;
            numberOfMinesDefused = 0;

            clearBoard();
            dropMinesRandomly();
            initialisePlayer();

            do {
                displayBoard();
                movePlayer();
            }
            while (numberOfMinesDefused < 3 && endOfGame == false);

            if (currentPlayerYPosition == boardHeight-1) {
                Console.WriteLine("*** You have won!  Congratulations on reaching the other side of the board ***");
            } else {
                Console.WriteLine("*** You have lost the game ***");
            }

            Console.WriteLine("Press Y to play again or any other key to exit");
            if (Console.ReadLine().ToUpper() == "Y") {
                Main();
            }
        }
    }
}