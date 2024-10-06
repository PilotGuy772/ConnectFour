using System.Runtime.CompilerServices;

class Board : IDeepCloneable<Board>
{
    public struct Cell
    {
        public Team Color;
        public Directions[] Affects;

        public Cell(int x, int y)
        {
            List<Directions> add = new List<Directions>
            { // list of all directions 
                Directions.North,
                Directions.East,
                Directions.South,
                Directions.West,
                Directions.Northeast,
                Directions.Northwest,
                Directions.Southeast,
                Directions.Southwest
            };

            if (x < 4) // remove directions from list if they are too close to the border to form a connect-four in the given direction
            {
                add.Remove(Directions.West);
                add.Remove(Directions.Northwest);
                add.Remove(Directions.Southwest);
            }
            if (x > 4)
            {
                add.Remove(Directions.East);
                add.Remove(Directions.Northeast);
                add.Remove(Directions.Southeast);
            }
            if (y < 4)
            {
                add.Remove(Directions.South);
                add.Remove(Directions.Southeast);
                add.Remove(Directions.Southwest);
            }
            else
            {
                add.Remove(Directions.North);
                add.Remove(Directions.Northeast);
                add.Remove(Directions.Northwest);
            }

            Affects = add.ToArray(); // take what remains and make it into an array



            Color = Team.None;
        }
    }

    public static int MAX_SEARCH_DEPTH = 5;

    public Cell[,] Grid { get; set; }
    public int[] ColumnCounter { get; set; }

    public Board()
    {
        ColumnCounter = [0, 0, 0, 0, 0, 0, 0];

        Grid = new Cell[7, 6];
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                Grid[x, y] = new Cell(x + 1, y + 1);
            }
        }
    }

    public static (int, int) AdvanceInDirection(Directions dir, (int x, int y) coords) //effectively allows you to convert a direction into a vector by giving you coordinates to the cell one unit in the given direction
    {
        switch (dir)
        {
            case Directions.North:
                return (coords.x, coords.y + 1);

            case Directions.East:
                return (coords.x + 1, coords.y);

            case Directions.South:
                return (coords.x, coords.y - 1);

            case Directions.West:
                return (coords.x - 1, coords.y);

            case Directions.Northeast:
                return (coords.x + 1, coords.y + 1);

            case Directions.Northwest:
                return (coords.x - 1, coords.y + 1);

            case Directions.Southeast:
                return (coords.x + 1, coords.y - 1);

            case Directions.Southwest:
                return (coords.x - 1, coords.y - 1);
            default:
                return (coords.x, coords.y);
        }
    }

    private static float CurveScore(float input) => 
        (float)( Math.Atan((double)(input / 100)) * (20 * Math.PI) ); //arctan function with an asymptote at y = 100
    

    public float EvaluateScore() //given a board, evaluate its score
    {
        (int x, int y) next;
        Team color;
        float rawScore = (float)ColumnCounter.Where(count => count == 6).Aggregate(0.0m, (current, i) => current + (int)ScoreCriteria.ImpossibleMove);


        for(int x = 0; x < 7; x++)
        {
            for(int y = 0; y < 6; y++)
            {

                //iterating through each cell

                //checks
                if(Grid[x, y].Color == Team.None)//if empty
                {
                    continue;//skip
                }

                color = Grid[x, y].Color;

                foreach(Directions dir in Grid[x, y].Affects)
                {
                    next = AdvanceInDirection(dir, (x, y));
                    if(Grid[next.x, next.y].Color != color) //if this isn't at least a two-in-a-row
                    {
                        continue; //skip it
                    }

                    next = AdvanceInDirection(dir, next);
                    if(Grid[next.x, next.y].Color == Team.None)
                    { //blockade

                        if(color == Team.Yellow) rawScore += (int)ScoreCriteria.OpponentBlockade; //add the appropriate item
                        if(color == Team.Red) rawScore += (int)ScoreCriteria.Blockade;

                    }else if(Grid[next.x, next.y].Color == color)//three in a row
                    { 
                        next = AdvanceInDirection(dir, next);

                        if(Grid[next.x, next.y].Color == Team.None) //threat
                        {
                            if(next.y == 0 || Grid[next.x, next.y - 1].Color != Team.None) //if there is a populated cell below, it's an attack
                            {

                                if(color == Team.Yellow) rawScore += (int)ScoreCriteria.OpponentAttack; //add the appropriate item
                                if(color == Team.Red) rawScore += (int)ScoreCriteria.Attack;

                            }else //otherwise it's just a regular threat
                            {

                                if(color == Team.Yellow) rawScore += (int)ScoreCriteria.OpponentThreat; //add the appropriate item
                                if(color == Team.Red) rawScore += (int)ScoreCriteria.Threat;

                            }
                            

                        }else if(Grid[next.x, next.y].Color == color) //four-in-a-row
                        {

                            if(color == Team.Yellow) return -100.0f; //return the appropriate score
                            if(color == Team.Red) return 100.0f;

                        }
                    }
                }
                
            }
        }//for loop

        //generate score
        return CurveScore(rawScore);

    }

    public static float Minimax(Board board, int depth, Team turn, float alpha, float beta)
    {
        //Console.WriteLine("\nStarted new search as player " + turn);
        //Console.WriteLine("Current board state:");
        //board.OutputBoard();
        
        if(depth == MAX_SEARCH_DEPTH)
        {
            return board.EvaluateScore();
        }

        Team? victory = board.CheckVictory();
        // check for not null
        // if victory is null, the board is not in a game over state
        if(victory is { } winner)
        {
            switch (winner)
            {
                case Team.Yellow:
                    return -100.0f;
                case Team.Red:
                    return 100.0f;
                case Team.None:
                    return 0.0f;
            }
        }

        Board[] moves = new Board[7];
               
        for (int i = 0; i < 7; i++)
        {
            if (board.ColumnCounter[i] == 6) continue;
            moves[i] = board.DeepClone();

            moves[i].Grid[i, moves[i].ColumnCounter[i]].Color = turn;
            moves[i].ColumnCounter[i]++;
        }


        float best;

        if(turn == Team.Yellow) //MIN side
        {
            best = 101;

            
            foreach(Board b in moves.Where(b => b is not null))
            {
                best = Math.Min(best, Minimax(b, depth + 1, turn.Swap(), alpha, beta));
                if(best < beta)
                {
                    beta = best; 
                } 

                if(best <= alpha) 
                {

                   break; //alpha-beta pruning
                }
            }

            
        }else //max side
        {
            best = -101;


            foreach(Board b in moves.Where(b => b is not null))
            {
                best = Math.Max(best, Minimax(b, depth + 1, turn.Swap(), alpha, beta));   
                if(best > alpha)
                {
                    alpha = best;
                }             
                if(best >= beta)
                {

                    break;
                }
            }

            
        }

        //Console.WriteLine("Chose child with score " + best);
        return best;
    }

    public Board DeepClone()
    {
        Cell[,] newGrid = new Cell[7, 6];
        for(int x = 0; x < 7; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                newGrid[x,y] = Grid[x, y];
            }
        }

        int[] newCol = new int[7];

        for(int x = 0; x < 7; x++)
        {
            newCol[x] = ColumnCounter[x];
        }

        return new Board
        {
            Grid = newGrid,
            ColumnCounter = newCol
        };

    }

    public void OutputBoard()
    {
        const string TOP = @"\\\\ 1 \\ 2 \\ 3 \\ 4 \\ 5 \\ 6 \\ 7 \\ ";
        const string BOTTOM = @"  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
        const string CAP = @"\\";
        const string SPACER = @" \\                                   \\";

        Console.Write(TOP + "\n");
        for (int y = 5; y >= 0; y--)
        {
            Console.Write(@" \\");

            for (int x = 0; x < 7; x++)
            {
                ConsoleColor color;
                bool pop;
                switch (Grid[x, y].Color)
                {
                    case Team.Red:
                        color = ConsoleColor.Red;
                        pop = true;
                        break;
                    case Team.Yellow:
                        color = ConsoleColor.Yellow;
                        pop = true;
                        break;
                    default:
                        color = Console.ForegroundColor;
                        pop = false;
                        break;
                }
                Program.ColorWrite(" [", color);
                if (pop)
                {
                    Program.ColorWrite("█] ", color);
                }
                else
                {
                    Program.ColorWrite(" ] ", color);
                }

            }
            Console.Write(CAP + "\n");
            if (y != 0)
            {
                Console.Write(SPACER + "\n");
            }

        }
        Console.Write(BOTTOM + "\n");
    }

    public Team? CheckVictory() //check if either team has won yet
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 7; x++) //iterate through every cell
            {
                if (Grid[x, y].Color == Team.None)
                {
                    continue;
                }

                foreach (Directions dir in Grid[x, y].Affects) //look in every direction that may lead to a connect-four from that cell
                {
                    (int x, int y) newPos = AdvanceInDirection(dir, (x, y)); //find the cell next to it in that direction
                    Team color = Grid[x, y].Color;
                    int counter = 1;
                    

                    while (true)
                    {
                        if (counter == 4) //if we have four in a row, it's a win
                        {
                            return color;
                        }

                        if (Grid[newPos.x, newPos.y].Color == color) //check if the new cell is the same color as the previous one
                        {
                            newPos = AdvanceInDirection(dir, (newPos.x, newPos.y)); //continue to the next cell in the same direction
                            counter++;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
        }

        // nobody has won yet
        foreach(int c in ColumnCounter) if(c != 6) return null;

        // it's a draw, everybody loses
        return Team.None;
    }
}