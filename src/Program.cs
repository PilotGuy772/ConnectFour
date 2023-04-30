using System;

class Program
{

    public static bool VERBOSE = false;
    public static bool CLEARSCREEN = false;
    public static bool PLAYER_FIRST = true;

    public static void Main(string[] args)
    {
        Board master = new Board();

        while(true)
        {

            Console.Clear();
            Console.WriteLine("Welcome to Connect Four\n");
            master.OutputBoard();
            Console.WriteLine("\n");
            ColorWrite("  (1)", ConsoleColor.Blue);Console.Write(" - Tree Search Depth - " + Board.MAX_SEARCH_DEPTH + "\n");
            ColorWrite("  (2)", ConsoleColor.Blue);Console.Write(" - Player Goes First - " + PLAYER_FIRST + "\n");
            ColorWrite("  (3)", ConsoleColor.Blue);Console.Write(" - Verbose Information - " + VERBOSE + "\n");
            ColorWrite("  (4)", ConsoleColor.Blue);Console.Write(" - Clear Screen Each Turn - " + CLEARSCREEN + "\n");
            ColorWrite("  (5)", ConsoleColor.Blue);Console.Write(" - PLAY\n\n");

            char key = Console.ReadKey().KeyChar;
            bool leave = false;
            string input;

            switch(key)
            {
                case '1':
                    Console.Clear();
                    Console.WriteLine("Welcome to Connect Four\n");
                    master.OutputBoard();
                    Console.WriteLine("\n");
                    ColorWrite("  (1)", ConsoleColor.Red);Console.Write(" - Tree Search Depth - " + Board.MAX_SEARCH_DEPTH + "\n");
                    ColorWrite("  (2)", ConsoleColor.Blue);Console.Write(" - Player Goes First - " + PLAYER_FIRST + "\n");
                    ColorWrite("  (3)", ConsoleColor.Blue);Console.Write(" - Verbose Information - " + VERBOSE + "\n");
                    ColorWrite("  (4)", ConsoleColor.Blue);Console.Write(" - Clear Screen Each Turn - " + CLEARSCREEN + "\n");
                    ColorWrite("  (5)", ConsoleColor.Blue);Console.Write(" - PLAY\n\n");

                    Console.Write("Input a new value -> ");
                    input = Console.ReadLine() ?? "";
                    int number;
                    try
                    {
                        number = Convert.ToInt32(input);
                    }catch{
                        continue;
                    }
                    Board.MAX_SEARCH_DEPTH = number;
                    continue;
                case '2':
                    Console.Clear();
                    Console.WriteLine("Welcome to Connect Four\n");
                    master.OutputBoard();
                    Console.WriteLine("\n");
                    ColorWrite("  (1)", ConsoleColor.Blue);Console.Write(" - Tree Search Depth - " + Board.MAX_SEARCH_DEPTH + "\n");
                    ColorWrite("  (2)", ConsoleColor.Red);Console.Write(" - Player Goes First - " + PLAYER_FIRST + "\n");
                    ColorWrite("  (3)", ConsoleColor.Blue);Console.Write(" - Verbose Information - " + VERBOSE + "\n");
                    ColorWrite("  (4)", ConsoleColor.Blue);Console.Write(" - Clear Screen Each Turn - " + CLEARSCREEN + "\n");
                    ColorWrite("  (5)", ConsoleColor.Blue);Console.Write(" - PLAY\n\n");

                    Console.Write("Select T or F -> ");
                    input = Console.ReadLine() ?? "";
                    if(input.ToLower() == "t")
                    {
                        PLAYER_FIRST = true;
                    }else if(input.ToLower() == "f")
                    {
                        PLAYER_FIRST = false;
                    }

                    continue;
                case '3':
                    Console.Clear();
                    Console.WriteLine("Welcome to Connect Four\n");
                    master.OutputBoard();
                    Console.WriteLine("\n");
                    ColorWrite("  (1)", ConsoleColor.Blue);Console.Write(" - Tree Search Depth - " + Board.MAX_SEARCH_DEPTH + "\n");
                    ColorWrite("  (2)", ConsoleColor.Blue);Console.Write(" - Player Goes First - " + PLAYER_FIRST + "\n");
                    ColorWrite("  (3)", ConsoleColor.Red);Console.Write(" - Verbose Information - " + VERBOSE + "\n");
                    ColorWrite("  (4)", ConsoleColor.Blue);Console.Write(" - Clear Screen Each Turn - " + CLEARSCREEN + "\n");
                    ColorWrite("  (5)", ConsoleColor.Blue);Console.Write(" - PLAY\n\n");


                    Console.Write("Select T or F -> ");
                    input = Console.ReadLine() ?? "";
                    if(input.ToLower() == "t")
                    {
                        VERBOSE = true;
                    }else if(input.ToLower() == "f")
                    {
                        VERBOSE = false;
                    }
                    break;
                case '4':
                    Console.Clear();
                    Console.WriteLine("Welcome to Connect Four\n");
                    master.OutputBoard();
                    Console.WriteLine("\n");
                    ColorWrite("  (1)", ConsoleColor.Blue);Console.Write(" - Tree Search Depth - " + Board.MAX_SEARCH_DEPTH + "\n");
                    ColorWrite("  (2)", ConsoleColor.Blue);Console.Write(" - Player Goes First - " + PLAYER_FIRST + "\n");
                    ColorWrite("  (3)", ConsoleColor.Blue);Console.Write(" - Verbose Information - " + VERBOSE + "\n");
                    ColorWrite("  (4)", ConsoleColor.Red);Console.Write(" - Clear Screen Each Turn - " + CLEARSCREEN + "\n");
                    ColorWrite("  (5)", ConsoleColor.Blue);Console.Write(" - PLAY\n\n");


                    Console.Write("Select T or F -> ");
                    input = Console.ReadLine() ?? "";
                    if(input.ToLower() == "t")
                    {
                        CLEARSCREEN = true;
                    }else if(input.ToLower() == "f")
                    {
                        CLEARSCREEN = false;
                    }   
                    break;
                case '5':
                    leave = true;
                    break;
                default:
                    continue;
            }

            if(leave == true) {break;}
        }
        
        Console.Clear();
        try
        {
            if(PLAYER_FIRST)
            {
                PlayPvAI_PlayerFirst(master);
            }else
            {
                PlayPvAI_AIFirst(master);
            }
        }catch(Exception e)
        {
            ColorWrite(e.Message + "\n", ConsoleColor.Red);
            Console.WriteLine(e.StackTrace);
        }
        
    }

    public static void ColorWrite(string text, ConsoleColor color)
    {
        var orig = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = orig;
    }

    public static void PlayPvP(Board board)
    {
        if(CLEARSCREEN) Console.Clear();
        bool yellowTurn = true;
        int[] columns = new int[7] {0, 0, 0, 0, 0, 0, 0}; //tracks the CURRENT NUMBER of populated cells in each column. Also conveniently tracks which Y value to add the next cell that is dropped
        var defColor = Console.ForegroundColor;

        while(true) // main loop
        {
            board.OutputBoard();

            if(yellowTurn)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nYELLOW'S TURN");
            }else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nRED'S TURN");
            }
            Console.Write("\n Choose the column to drop your piece\n\n");
            Console.ForegroundColor = Console.BackgroundColor;

            char column = Console.ReadKey().KeyChar;Console.Write("\n");
            Console.ForegroundColor = defColor;
            int num = 0;

            switch(column)
            {

                case '1':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '2':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '3':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '4':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '5':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '6':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }

                    columns[num]++;
                    break;
                case '7':
                    num = Convert.ToInt32(column.ToString()) - 1;
                    if(columns[num] == 7)
                    {
                        continue;
                    }            
                    if(yellowTurn == true)
                    {
                        board.Grid[num, columns[num]].Color = Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Team.Red;
                    }     

                    columns[num]++;   
                    break;

                case 'q':
                    return;
                case 'Q':
                    return;

                default:
                    continue;                
            }

            if(board.CheckVictory())
            {
                board.OutputBoard();
                break;
            }
            if(yellowTurn == true)
            {
                yellowTurn = false;
            }else
            {
                yellowTurn = true;
            }
        }

        Console.WriteLine("\n\ngame over y'all");
    }

    public static void PlayPvAI_PlayerFirst(Board board)
    {
        while(true) //main loop
        {
            if(CLEARSCREEN) Console.Clear();
            board.OutputBoard();
            if(board.CheckVictory())
            {
                return;
            }

            ColorWrite("\n\nYELLOW'S TURN\n    Please select the column to drop your piece\n\n", ConsoleColor.Yellow);

            char key = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            int num;

            switch(key)
            {

                case '1':
                    num = 0;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '2':
                    num = 1;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '3':
                    num = 2;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '4':
                    num = 3;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '5':
                    num = 4;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '6':
                    num = 5;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '7':
                    num = 6;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                default:
                    continue;                
            }   

            if(CLEARSCREEN) Console.Clear();            
            
            board.OutputBoard();    

            ColorWrite("\n\nRED'S TURN\n    The computer is thinking...\n\n", ConsoleColor.Red);

            //complete preliminary checks => check for an attack from yellow, check for a case where only one move is possible, check if the center column has a piece in it
            for(int x = 0; x < 7; x++)
            {
                for(int y = 0; y < 6; y++)
                {
                    if (board.Grid[x, y].Color == Team.None) //if this cell is empty, continue
                    {
                        continue;
                    }

                    Team color = board.Grid[x, y].Color;
                    foreach (Directions dir in board.Grid[x, y].Affects) //iterate through each direction
                    {
                        (int x, int y) next = Board.AdvanceInDirection(dir, (x, y)); //get the nearest cell in that direction
                        if (board.Grid[next.x, next.y].Color == color) //if it's followed by another cell of the same color 
                        {
                            next = Board.AdvanceInDirection(dir, next); //continue in the same direction
                            if (board.Grid[next.x, next.y].Color == color) //if by yet another cell of the same color (third)
                            {
                                next = Board.AdvanceInDirection(dir, next);
                                if (board.Grid[next.x, next.y].Color == Team.None) //it might be a threat, but only if it's followed by an empty cell
                                { //yeah it's a threat
                                    if(next.y == 0 || board.Grid[next.x, next.y - 1].Color != Team.None) //if there is a populated cell below (or a border)...
                                    {
                                        if(color == Team.Yellow) //if the opponent is directly threatening the AI,
                                        {
                                            board.Grid[next.x, board.ColumnCounter[next.x]].Color = Team.Red; //respond accordingly
                                            board.ColumnCounter[next.x]++;
                                            continue;
                                        }
                                    }
                                }else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            
            //check for possible moves based on column fullness

            List<int> legalMoves = new List<int>();
            for(int c = 0; c < 7; c++)
            {
                if(board.ColumnCounter[c] != 6)
                {
                    legalMoves.Add(c);
                }
            }


            //if only one move is possible, take it
            if(legalMoves.Count == 1)
            {
                board.Grid[legalMoves[0], board.ColumnCounter[legalMoves[0]]].Color = Team.Red;
                board.ColumnCounter[legalMoves[0]]++;
                continue;
            }else if(legalMoves.Count == 0) //if zero moves are possible, return (it's a draw)
            {
                return;
            }

            //duplicate boards
            List<Board> newBoards = new List<Board>();
            
            

            for(int x = 0; x < legalMoves.Count; x++)
            {
                Board b = board.DeepClone();
                b.Grid[legalMoves[x], b.ColumnCounter[legalMoves[x]]].Color = Team.Red;
                newBoards.Add(b);
            }


            bool skipThisBoard;
            List<int> returnedScores = new List<int>();
            int givenScore;
            foreach(Board working in newBoards)
            {
                skipThisBoard = false;

                
                
                if(skipThisBoard)
                {
                    returnedScores.Add(-1);
                    continue;
                }

                if(VERBOSE) Console.WriteLine("Evaluating move....");
                givenScore = Board.EvaluateScore(1, working, Team.Red);
                returnedScores.Add(givenScore);
                if(VERBOSE) Console.WriteLine("Returned with score rating of {0}", givenScore);
            }

            (int val, int index) highest = (0, 0);
            for(int i = 0; i < returnedScores.Count; i++)
            {
                if(returnedScores[i] > highest.val && returnedScores[i] != -1)
                {
                    Console.WriteLine("{0} is larger than {1}", returnedScores[i], highest);
                    highest = (returnedScores[i], i);
                }
            }

            board.Grid[legalMoves[highest.index], board.ColumnCounter[legalMoves[highest.index]]].Color = Team.Red;
            board.ColumnCounter[legalMoves[highest.index]]++;

            if(VERBOSE) Console.WriteLine("Moved in column {0}, array index {1}, score rating of {2}", legalMoves[highest.index], highest.index, highest.val);
        }
    }

    public static void PlayPvAI_AIFirst(Board board)
    {
        while(true) //main loop
        {
            if(CLEARSCREEN) Console.Clear();            
            board.OutputBoard();
            if(board.CheckVictory())
            {
                return;
            }

            board.OutputBoard();    


            ColorWrite("\n\nRED'S TURN\n    The computer is thinking...\n\n", ConsoleColor.Red);

            //complete preliminary checks => check for an attack from yellow, check for a case where only one move is possible, check if the center column has a piece in it
            
            //check for possible moves based on column fullness

            List<int> legalMoves = new List<int>();
            for(int c = 0; c < 7; c++)
            {
                if(board.ColumnCounter[c] != 6)
                {
                    legalMoves.Add(c);
                }
            }
            Console.WriteLine();

            //if only one move is possible, take it
            if(legalMoves.Count == 1)
            {
                board.Grid[legalMoves[0], board.ColumnCounter[legalMoves[0]]].Color = Team.Red;
                board.ColumnCounter[legalMoves[0]]++;
                continue;
            }else if(legalMoves.Count == 0) //if zero moves are possible, return (it's a draw)
            {
                return;
            }

            //duplicate boards
            List<Board> newBoards = new List<Board>();
            
            

            for(int x = 0; x < legalMoves.Count; x++)
            {
                Board b = board.DeepClone();
                b.Grid[legalMoves[x], b.ColumnCounter[legalMoves[x]]].Color = Team.Red;
                newBoards.Add(b);
            }

            //check each board for suicide moves
            bool skipThisBoard;
            List<int> returnedScores = new List<int>();
            int givenScore;
            foreach(Board working in newBoards)
            {
                skipThisBoard = false;

                
                
                if(skipThisBoard)
                {
                    returnedScores.Add(-1);
                    continue;
                }

                if(VERBOSE) Console.WriteLine("Evaluating move....");
                givenScore = Board.EvaluateScore(1, working, Team.Red);
                returnedScores.Add(givenScore);
                if(VERBOSE) Console.WriteLine("Returned with score rating of {0}", givenScore);
            }

            (int val, int index) highest = (0, 0);
            for(int i = 0; i < returnedScores.Count; i++)
            {
                if(returnedScores[i] > highest.val && returnedScores[i] != -1)
                {
                    highest = (returnedScores[i], i);
                }
            }

            board.Grid[legalMoves[highest.index], board.ColumnCounter[legalMoves[highest.index]]].Color = Team.Red;
            board.ColumnCounter[legalMoves[highest.index]]++;

            if(VERBOSE) Console.WriteLine("Moved in column {0}, array index {1}, score rating of {2}", legalMoves[highest.index], highest.index, highest.val);

            if(CLEARSCREEN) Console.Clear();
            board.OutputBoard();

            ColorWrite("\n\nYELLOW'S TURN\n    Please select the column to drop your piece\n\n", ConsoleColor.Yellow);

            char key = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            int num;

            switch(key)
            {

                case '1':
                    num = 0;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '2':
                    num = 1;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '3':
                    num = 2;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '4':
                    num = 3;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '5':
                    num = 4;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '6':
                    num = 5;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                case '7':
                    num = 6;
                    if(board.ColumnCounter[num] == 7)
                    {
                        continue;
                    }
                        board.Grid[num, board.ColumnCounter[num]].Color = Team.Yellow;
                    board.ColumnCounter[num]++;
                    break;

                default:
                    continue;                
            }   

        }
    }
}