using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CrossMutateStrategy : Strategy
{
    public override ShipGenome GetGenome()
    {
        ShipGenome genome;
        var leaderBoard = GetLeaderBoard();
        if (leaderBoard.Count > 0)
        {
            genome = GetLeaderBoardGenome(leaderBoard);
            ShipGenome crossGenome = GetLeaderBoardGenome(leaderBoard);
            genome.Cross(crossGenome);
        }
        else
        {
            genome = Instantiate(defaultGenome);
        }

        return genome.Mutate();
    }
}
