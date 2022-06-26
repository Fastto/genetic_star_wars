using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MutateOnlyStrategy : Strategy
{
    public override ShipGenome GetGenome()
    {
        var leaderBoard = GetLeaderBoard();
        ShipGenome genome = leaderBoard.Count > 0
            ? GetLeaderBoardGenome(leaderBoard)
            : Instantiate(defaultGenome);
        return genome.Mutate();
    }
}
