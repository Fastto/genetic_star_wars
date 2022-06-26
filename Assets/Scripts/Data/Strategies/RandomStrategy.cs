using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomStrategy : Strategy
{
    public override ShipGenome GetGenome()
    {
        GetLeaderBoard();
        return ShipGenome.GetRandom();
    }
}
