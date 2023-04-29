using System;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("sup let's see our awesome board");
        Board master = new Board();
        try
        {
            PlayPvP(master);
        }catch(Exception e)
        {
            ColorWrite(e.Message, ConsoleColor.Red);
            Console.WriteLine(e.TargetSite);
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
                        board.Grid[num, columns[num]].Color = Board.Team.Yellow;
                    }else
                    {
                        board.Grid[num, columns[num]].Color = Board.Team.Red;
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
}