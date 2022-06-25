using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CrossMutateStrategy : Strategy
{
    [SerializeField] private float initialProbability;
    [SerializeField] private float probabilityDownRate;
    public override ShipGenome GetGenome()
    {
        ShipGenome genome = Instantiate(defaultGenome);
        
        var leaderBoard = GetLeaderBoard();
        var leaderBoardSize = leaderBoard.Count;

        Debug.Log(leaderBoardSize);
        if (leaderBoardSize > 0)
        {
            bool genomeFound = false;
            float probability = initialProbability;
            
            int minId = leaderBoardSize - 1 - LeaderBoardSize;
            minId = minId < 0 ? 0 : minId;
            int maxId = leaderBoardSize - 1;

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
                    genome = Instantiate(leaderBoard.Values[Random.Range(minId, maxId)]);
                }
            }

            ShipGenome crossGenome = leaderBoard.Values[Random.Range(minId, maxId)];
            genome.Cross(crossGenome);
        }

        return genome.Mutate();
    }
}
