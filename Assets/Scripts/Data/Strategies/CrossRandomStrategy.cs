using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu]
public class CrossRandomStrategy : Strategy
{
    [SerializeField] private float randomProbability;

    public override ShipGenome GetGenome()
    {
        ShipGenome genome;
        var leaderBoard = GetLeaderBoard();
        if (leaderBoard.Count > 0)
        {
            genome = GetLeaderBoardGenome(leaderBoard);
            ShipGenome crossGenome= Random.value < randomProbability 
                ? ShipGenome.GetRandom()
                : GetLeaderBoardGenome(leaderBoard);
            genome.Cross(crossGenome);
        }
        else
        {
            genome = Instantiate(defaultGenome);
        }

        return genome;
    }
}