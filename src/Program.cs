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

            if(board.CheckVictory() is not null)
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
            if(board.CheckVictory() is not null)
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

            //check possible moves
            List<int> noMove = new List<int>();
            for(int i = 0; i < 7; i++) if(board.ColumnCounter[i] == 6) noMove.Add(i); //add full columns to the noMove list

            float[] scores = new float[7]; //track scores
            Board dupe;
            for(int i = 0; i < 7; i++)
            {
                if(noMove.Contains(i)) //if this is an illegal move, give it a super low score so that it is not chosen
                {
                    scores[i] = -101.0f;
                    if(VERBOSE) Console.WriteLine("Skipped evaluation of column {0}", i);
                    continue;
                }

                dupe = board.DeepClone(); //duplicate board
                dupe.Grid[i, dupe.ColumnCounter[i]].Color = Team.Red; //make the move
                dupe.ColumnCounter[i]++;
                if(VERBOSE) Console.WriteLine("Evaluating column {0}", i);
                scores[i] = Board.Minimax(dupe, 1, Team.Yellow, -101, 101); //call minimax and set the corresponding spot in the score tracker to the returned score
                if(VERBOSE) Console.WriteLine("Complete! Returned with score {0}", scores[i]);
            }

            float highscore = -102;
            int highscoreIndex = -1; //find the index and value of the highest returned score
            for(int i = 0; i < 7; i++)
            {
                if(scores[i] > highscore)
                {
                    highscore = scores[i];
                    highscoreIndex = i;
                }
            }

            if(highscore == -102 | highscoreIndex == -1)
            {
                Console.WriteLine("\n\n IT'S A DRAW\n\n");
                return;
            }

            if(VERBOSE) Console.WriteLine("MOVE SELECTED: COL {0} WITH SCORE {1}", highscoreIndex, highscore);
            board.Grid[highscoreIndex, board.ColumnCounter[highscoreIndex]].Color = Team.Red;
            board.ColumnCounter[highscoreIndex]++;

        }
    }

    public static void PlayPvAI_AIFirst(Board board)
    {
        while(true) //main loop
        {
            if(CLEARSCREEN) Console.Clear();            
            board.OutputBoard();
            if(board.CheckVictory() is not null)
            {
                return;
            }

            board.OutputBoard();    


            ColorWrite("\n\nRED'S TURN\n    The computer is thinking...\n\n", ConsoleColor.Red);

            //check possible moves
            List<int> noMove = new List<int>();
            for(int i = 0; i < 7; i++) if(board.ColumnCounter[i] == 6) noMove.Add(i); //add full columns to the noMove list

            float[] scores = new float[7]; //track scores
            Board dupe;
            for(int i = 0; i < 7; i++)
            {
                if(noMove.Contains(i)) //if this is an illegal move, give it a super low score so that it is not chosen
                {
                    scores[i] = -101.0f;
                    if(VERBOSE) Console.WriteLine("Skipped evaluation of column {0}", i);
                    continue;
                }

                dupe = board.DeepClone(); //duplicate board
                dupe.Grid[i, dupe.ColumnCounter[i]].Color = Team.Red; //make the move
                dupe.ColumnCounter[i]++;
                if(VERBOSE) Console.WriteLine("Evaluating column {0}", i);
                scores[i] = Board.Minimax(dupe, 1, Team.Yellow, -110, 110); //call minimax and set the corresponding spot in the score tracker to the returned score
                if(VERBOSE) Console.WriteLine("Complete! Returned with score {0}", scores[i]);
            }

            float highscore = -102;
            int highscoreIndex = -1; //find the index and value of the highest returned score
            for(int i = 0; i < 7; i++)
            {
                if(scores[i] > highscore)
                {
                    highscore = scores[i];
                    highscoreIndex = i;
                }
            }

            if(highscore == -102 | highscoreIndex == -1)
            {
                Console.WriteLine("\n\n IT'S A DRAW\n\n");
                return;
            }

            if(VERBOSE) Console.WriteLine("MOVE SELECTED: COL {0} WITH SCORE {1}", highscoreIndex, highscore);
            board.Grid[highscoreIndex, board.ColumnCounter[highscoreIndex]].Color = Team.Red;
            board.ColumnCounter[highscoreIndex]++;
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