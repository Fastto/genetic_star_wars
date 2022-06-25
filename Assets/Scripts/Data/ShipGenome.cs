using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipGenome : ScriptableObject
{
   public static readonly int MinCapacity = 1;
   public static readonly int MaxCapacity = 20;
   
   public static readonly int MinDamage = 1;
   public static readonly int MaxDamage = 20;
   
   public static readonly int MinHealth = 4;
   public static readonly int MaxHealth = 80;
   
   [Range(0f, 1f)]
   public float cargoGen;
   
   [Range(0f, 1f)]
   public float damageGen;

   public static ShipGenome GetRandom()
   {
      var genome = ScriptableObject.CreateInstance<ShipGenome>();
      genome.cargoGen = Random.Range(0, 1f);
      genome.damageGen = Random.Range(0, 1f);

      return genome;
   }

   public int GetCapacity()
   {
      int capacity = Mathf.RoundToInt(MaxCapacity * cargoGen);
      return capacity == 0 ? MinCapacity : capacity;
   }
   
   public float GetCapacityPoints()
   {
      return cargoGen;
   }

   public int GetDamage()
   {
      float warPoints = 1f - cargoGen;
      int damage = Mathf.RoundToInt(MaxDamage * damageGen * warPoints);
      return damage == 0 ? MinDamage : damage;
   }
   
   public float GetDamagePoints()
   {
      float warPoints = 1f - cargoGen;
      return damageGen * warPoints;
   }

   public int GetHealth()
   {
      float warPoints = 1f - cargoGen;
      float healthPoints = 1f - damageGen;
      int health = Mathf.RoundToInt(MaxHealth * healthPoints * warPoints);
      return health == 0 ? MinHealth : health;
   }
   
   public float GetHealthPoints()
   {
      float warPoints = 1f - cargoGen;
      float healthPoints = 1f - damageGen;
      return healthPoints * warPoints;
   }
}
