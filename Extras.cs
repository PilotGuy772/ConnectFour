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
    OpponentForcedHand = 25,
    Blockade = 10,
    Threat = 30,
    Attack = 50,
    ForcedWin = 125,
    ImpossibleMove = -15,
    OpponentBlockade = -10,
    OpponentThreat = -30,
    OpponentAttack = -100,
    OpponentWin = -125
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

//archived from Program.PlayPvAI

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
                */
