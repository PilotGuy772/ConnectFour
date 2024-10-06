interface IDeepCloneable<T>
{
    public T DeepClone();
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
enum ScoreCriteria
{
    Blockade = 20,
    Threat = 60,
    Attack = 60,

    ImpossibleMove = -30,
    OpponentBlockade = -25,
    OpponentThreat = -50,
    OpponentAttack = -60
}

public static class Extensions
{
    public static Team Swap(this Team team)
    {
        switch(team)
        {
            case Team.Red:
                return Team.Yellow;
            case Team.Yellow:
                return Team.Red;
            default:
                return Team.None;
        }
    }
}

//archived from Program.PlayPvAI()

/*+
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
                            (int x, int y) next = Board.AdvanceInDirection(dir, (x, y)); //get the nearest cell in that direction
                            if (working.Grid[next.x, next.y].Color == color) //if it's followed by another cell of the same color 
                            {
                                next = Board.AdvanceInDirection(dir, next); //continue in the same direction
                                if (working.Grid[next.x, next.y].Color == color) //if by yet another cell of the same color (third)
                                {
                                    next = Board.AdvanceInDirection(dir, next);
                                    if (working.Grid[next.x, next.y].Color == Team.None) //it might be a threat, but only if it's followed by an empty cell
                                    { //yeah it's a threat
                                        if(next.y == 0 || working.Grid[next.x, next.y - 1].Color != Team.None) //if there is a populated cell below (or a border)...
                                        {
                                            if(color == Team.Yellow) //If this is the active player putting himself in a situation where the opponent will win on the next move
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



old version of Board.EvaluateScore()

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
                                        continue;
                                    }else
                                    {
                                        if(turn == Team.Red) //likewise if it is red's turn and red has an attack, it is an inevitable win
                                        {
                                            return 125; //so it should return 125 immediately
                                        }
                                        logbook.Add(ScoreCriteria.Attack);
                                        logbook.Add(ScoreCriteria.OpponentForcedHand);
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


            foreach(Board working in newBoards)
            {
                givenScore = EvaluateScore(treeDepth + 1, working, turn.Swap());
                returnedScores.Add(givenScore);
            }
        }

        int sum = returnedScores.Sum() + score;
        return (int)Math.Round((float)(sum / (returnedScores.Count + 1)));

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


                */


