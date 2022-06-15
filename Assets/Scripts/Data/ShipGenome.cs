using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipGenome : ScriptableObject
{
   [Range(0f, 1f)]
   public float battleGen;
   
   [Range(0f, 1f)]
   public float damageGen;
}
