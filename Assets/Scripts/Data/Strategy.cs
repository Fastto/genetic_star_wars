using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy : ScriptableObject
{
    [SerializeField] protected ShipGenome defaultGenome;

    public virtual ShipGenome GetGenome()
    {
        return defaultGenome;
    }

    public virtual void RegisterShip(ShipController shipController)
    {
        
    }
}
