using System.Linq;

interface IDeepCloneable<T>
{
    public T DeepClone();
}

class Board
{
    public struct Cell
    {
        public Team Color;
        public Directions[] Affects;

        public Cell(int x, int y)
        {
            List<Directions> add = new List<Directions>() { // list of all directions 
                Directions.North,
                Directions.East, 
                Directions.South,
                Directions.West, 
                Directions.Northeast,
                Directions.Northwest,
                Directions.Southeast,
                Directions.Southwest 
            };

            if(x < 4) // remove directions from list if they are too close to the border to form a connect-four in the given direction
            {
                add.Remove(Directions.West);
                add.Remove(Directions.Northwest);
                add.Remove(Directions.Southwest);
            }
            if(x > 4)
            {
                add.Remove(Directions.East);
                add.Remove(Directions.Northeast);
                add.Remove(Directions.Southeast);
            }
            if(y < 4)
            {
                add.Remove(Directions.South);
                add.Remove(Directions.Southeast);
                add.Remove(Directions.Southwest);
            }else
            {
                add.Remove(Directions.North);
                add.Remove(Directions.Northeast);
                add.Remove(Directions.Northwest);
            }

            Affects = add.ToArray(); // take what remains and make it into an array



            Color = Team.None;
        }
    }
    public enum Team
    {
        Red,
        Yellow,
        None
    }
    public enum Directions
    {
        North, //  x, +y
        East,  // +x,  y
        South, //  x, -y
        West,  // -x,  y
        Northeast, // +x, +y
        Northwest, // -x, +y
        Southeast, // +x, -y
        Southwest  // -x, -y
    }


    public Cell[,] Grid {get; set;}

    public Board()
    {
        Grid = new Cell[7,6];
        for(int x = 0; x < 7; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                Grid[x,y] = new Cell(x+1, y+1);
            }
        }
    }

    public static (int, int) AdvanceInDirection(Directions dir, (int x, int y) coords) //effectively allows you to convert a direction into a vector by giving you coordinates to the cell one unit in the given direction
    {
        switch(dir)
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

    public void OutputBoard()
    {
        const string TOP = @"\\\\ 1 \\ 2 \\ 3 \\ 4 \\ 5 \\ 6 \\ 7 \\ ";
        const string BOTTOM = @"  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
        const string CAP = @"\\";
        const string SPACER = @" \\                                   \\";

        Console.Write(TOP + "\n");
        for(int y = 5; y >= 0; y--)
        {
            Console.Write(@" \\");
            
            for(int x = 0; x < 7; x++)
            {
                ConsoleColor color;
                bool pop;
                switch(this.Grid[x,y].Color)
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
                if(pop)
                {
                    Program.ColorWrite("█] ", color);
                }else
                {
                    Program.ColorWrite(" ] ", color);
                }
                
            }
            Console.Write(CAP + "\n");
            if(y != 0)
            {
                Console.Write(SPACER + "\n");
            }
                
        }
        Console.Write(BOTTOM + "\n");
    }

    public bool CheckVictory() //check if either team has won yet
    {
        for(int y = 0; y < 6; y++) 
        {
            for(int x = 0; x < 7; x++) //iterate through every cell
            {
                if(this.Grid[x, y].Color == Team.None)
                {
                    continue;
                }
                
                foreach(Directions dir in this.Grid[x,y].Affects) //look in every direction that may lead to a connect-four from that cell
                {
                    (int x, int y) newPos = AdvanceInDirection(dir, (x, y)); //find the cell next to it in that direction
                    Team color = this.Grid[x,y].Color;
                    int counter = 1;

                    while(true)
                    {
                        if(counter == 4) //if we have four in a row, it's a win
                        {
                            return true;
                        }

                        if(this.Grid[newPos.x, newPos.y].Color == color) //check if the new cell is the same color as the previous one
                        {
                            newPos = AdvanceInDirection(dir, (newPos.x, newPos.y)); //continue to the next cell in the same direction
                            counter++;
                        }else
                        {
                            break;
                        }

                    }                 
                }
            }
        }

        return false;
    }
}