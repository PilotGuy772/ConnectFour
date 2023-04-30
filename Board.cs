using System.Linq;


class Board : IDeepCloneable<Board>
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

    public static int MAX_SEARCH_DEPTH = 6;

    public Cell[,] Grid { get; set; }
    public int[] ColumnCounter { get; set; }

    public Board(Cell[,] grid, int[] columnCounter)
    {
        Grid = grid;
        ColumnCounter = columnCounter;
    }

    public Board()
    {
        ColumnCounter = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

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

    public static int EvaluateScore(int treeDepth, Board board, Team turn)
    {
        List<ScoreCriteria> logbook = new List<ScoreCriteria>();
        int forceMoveColumn = -1; //what column to move if the move is forced
        List<int> legalColumns = new List<int>(); //counts which columns are empty and can be moved in
        
        for(int c = 0; c < 6; c++)
        {
            if(board.ColumnCounter[c] != 6) //if there are any columns with space left in them,
            {
                legalColumns.Add(c); //add those columns to the legal columns list
            }else
            {
                logbook.Add(ScoreCriteria.ImpossibleMove); //if there is an illegal column, points must be deducted

            }
        }
        if(legalColumns.Count == 1) //in case of only one possible move
        {
            forceMoveColumn = legalColumns[0]; //the forced move will be this one
        }else if(legalColumns.Count == 0) //in case of a draw
        {
            return 0;// zero points
        }


        List<(int, int)> threatSpaces = new List<(int, int)>();
        //count enemy threats & blockades & attacks
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 7; x++) //iterate through every cell
            {
                if (board.Grid[x, y].Color == Team.None) //if this cell is empty, continue
                {
                    continue;
                }

                Team color = board.Grid[x, y].Color;
                foreach (Directions dir in board.Grid[x, y].Affects) //iterate through each direction
                {
                    (int x, int y) next = AdvanceInDirection(dir, (x, y)); //get the nearest cell in that direction
                    if (board.Grid[next.x, next.y].Color == color) //if it's followed by another cell of the same color 
                    {
                        next = AdvanceInDirection(dir, next); //continue in the same direction
                        if (board.Grid[next.x, next.y].Color == Team.None) //if it's followed again by an empty cell
                        { //then it's a blockade
                            if(color == Team.Yellow)
                            {
                                logbook.Add(ScoreCriteria.OpponentBlockade);
                                continue;
                            }else
                            {
                                logbook.Add(ScoreCriteria.Blockade);
                                continue;
                            }
                        }
                        if (board.Grid[next.x, next.y].Color == color) //if by yet another cell of the same color (third)
                        {
                            next = AdvanceInDirection(dir, next);
                            if (board.Grid[next.x, next.y].Color == Team.None) //it might be a threat, but only if it's followed by an empty cell
                            { //yeah it's a threat

                                if(threatSpaces.Contains(next))
                                {
                                    continue;
                                }
                                threatSpaces.Add(next);


                                if(next.y == 0 || board.Grid[next.x, next.y - 1].Color != Team.None) //if there is a populated cell below (or a border)...
                                {
                                    if(color == Team.Yellow) //it is an attack
                                    {
                                        if(turn == Team.Yellow) //if yellow has an attack and it is currently yellow's turn, it is an inevitable win (AI is always red)
                                        {
                                            return -125; //so it should return -125 immediately
                                        }
                                        logbook.Add(ScoreCriteria.OpponentAttack);
                                        forceMoveColumn = next.x; //the AI now must move in the column that the opponent would have to move to make a win
                                        continue;
                                    }else
                                    {
                                        if(turn == Team.Red) //likewise if it is red's turn and red has an attack, it is an inevitable win
                                        {
                                            return 125; //so it should return 125 immediately
                                        }
                                        logbook.Add(ScoreCriteria.Attack);
                                        logbook.Add(ScoreCriteria.OpponentForcedHand);
                                        forceMoveColumn = next.x; //likewise if it is the player's turn to move and the AI has an attack, the player will move to prevent it.
                                        continue;
                                    }
                                }

                                if(color == Team.Yellow) //otherwise it's just a threat
                                {
                                    logbook.Add(ScoreCriteria.OpponentThreat);
                                    continue;
                                }else
                                {
                                    logbook.Add(ScoreCriteria.Threat);
                                    continue;
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

        int score = 0;
        foreach(ScoreCriteria cr in logbook)
        {
            score += (int)cr; //convert to integers and add to the score counter
        }


        if(treeDepth == MAX_SEARCH_DEPTH)
        {
            return score;
        }


        List<int> returnedScores = new List<int>();
        Board[] newBoards;
        int givenScore;

        if(forceMoveColumn != -1) //if the system is forced to move somewhere
        {
            Board newBoard = board.DeepClone();
            newBoard.Grid[forceMoveColumn, newBoard.ColumnCounter[forceMoveColumn]].Color = turn;
            //            xcoord is cooumn,  where it lands is tracked in columnCounter     turns the color of whose turn it is
            newBoard.ColumnCounter[forceMoveColumn]++;
            givenScore = EvaluateScore(treeDepth + 1, newBoard, turn.Swap());
            returnedScores.Add(givenScore);
        }else //otherwise, proceed normally
        {

            newBoards = new Board[legalColumns.Count];
            for(int i = 0; i < legalColumns.Count; i++)
            {
                newBoards[i] = board.DeepClone();
                newBoards[i].Grid[legalColumns[i],newBoards[i].ColumnCounter[legalColumns[i]]].Color = turn; //make the move                
            } //make a new copy of the board for each move that will be made

            bool skipThisBoard;
            foreach(Board working in newBoards)
            {
                skipThisBoard = false;

                for(int x = 0; x < 7; x++)
                {
                    for(int y = 0; y < 6; y++)
                    {
                        if (working.Grid[x, y].Color == Team.None) //if this cell is empty, continue
                        {
                            continue;
                        }

                        Team color = working.Grid[x, y].Color;
                        foreach (Directions dir in working.Grid[x, y].Affects) //iterate through each direction
                        {
                            (int x, int y) next = AdvanceInDirection(dir, (x, y)); //get the nearest cell in that direction
                            if (working.Grid[next.x, next.y].Color == color) //if it's followed by another cell of the same color 
                            {
                                next = AdvanceInDirection(dir, next); //continue in the same direction
                                if (working.Grid[next.x, next.y].Color == color) //if by yet another cell of the same color (third)
                                {
                                    next = AdvanceInDirection(dir, next);
                                    if (working.Grid[next.x, next.y].Color == Team.None) //it might be a threat, but only if it's followed by an empty cell
                                    { //yeah it's a threat
                                        if(next.y == 0 || working.Grid[next.x, next.y - 1].Color != Team.None) //if there is a populated cell below (or a border)...
                                        {
                                            if(color != turn) //If this is the active player putting himself in a situation where the opponent will win on the next move
                                            {
                                                skipThisBoard = true;
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
                
                if(skipThisBoard)
                {
                    continue;
                }

                givenScore = EvaluateScore(treeDepth + 1, working, turn.Swap());
                returnedScores.Add(givenScore);
            }
        }

        int sum = returnedScores.Sum() + score;
        return (int)Math.Round((decimal)(sum / returnedScores.Count + 1));

    }

    public Board DeepClone()
    {
        Cell[,] newGrid = new Cell[7,6];
        int colorIndex;

        for(int x = 0; x < 7; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                colorIndex = (int)this.Grid[x, y].Color;

                newGrid[x, y] = new Cell() {
                    Color = (Team)colorIndex,
                    Affects = this.Grid[x, y].Affects
                };
            }
        }

        int[] columnCounter = new int[7] {0, 0, 0, 0, 0, 0, 0};
        for(int i = 0; i < 7; i++)
        {
            columnCounter[i] = this.ColumnCounter[i];
        }

        return new Board(newGrid, columnCounter);

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
                switch (this.Grid[x, y].Color)
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

    public bool CheckVictory() //check if either team has won yet
    {
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 7; x++) //iterate through every cell
            {
                if (this.Grid[x, y].Color == Team.None)
                {
                    continue;
                }

                foreach (Directions dir in this.Grid[x, y].Affects) //look in every direction that may lead to a connect-four from that cell
                {
                    (int x, int y) newPos = AdvanceInDirection(dir, (x, y)); //find the cell next to it in that direction
                    Team color = this.Grid[x, y].Color;
                    int counter = 1;

                    while (true)
                    {
                        if (counter == 4) //if we have four in a row, it's a win
                        {
                            return true;
                        }

                        if (this.Grid[newPos.x, newPos.y].Color == color) //check if the new cell is the same color as the previous one
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

        return false;
    }
}