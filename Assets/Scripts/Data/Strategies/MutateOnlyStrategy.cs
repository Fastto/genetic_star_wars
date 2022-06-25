using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MutateOnlyStrategy : Strategy
{
    [SerializeField] private float initialProbability;
    [SerializeField] private float probabilityDownRate;
    public override ShipGenome GetGenome()
    {
        ShipGenome genome = Instantiate(defaultGenome);
        
        var leaderBoard = GetLeaderBoard();
        var leaderBoardSize = leaderBoard.Count;

        if (leaderBoardSize > 0)
        {
            bool genomeFound = false;
            float probability = initialProbability;

            int id = 0;
            while (!genomeFound)
            {
                if (Random.value < probability)
                {
                    genomeFound = true;
                    genome = Instantiate(leaderBoard.Values[leaderBoardSize - id - 1]);
                }
                else
                {
                    id++;
                    probability *= probabilityDownRate;
                }

                if (!genomeFound && (id == leaderBoard.Count || id == LeaderBoardSize))
                {
                    genomeFound = true;
                    int minId = leaderBoardSize - 1 - LeaderBoardSize;
                    minId = minId < 0 ? 0 : minId;
                    int maxId = leaderBoardSize - 1;
                    genome = Instantiate(leaderBoard.Values[Random.Range(minId, maxId)]);
                }
            }
        }

        return genome.Mutate();
    }
}
