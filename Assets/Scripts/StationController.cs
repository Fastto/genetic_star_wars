using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
   [SerializeField] private ResourcesManager resourcesManager;
   [SerializeField] private Team team;

   [SerializeField] private GameObject shipPrefab;
   [SerializeField] private ShipGenome shipInitialGenome;

   [SerializeField] private int initialGold;
   [SerializeField] private int shipCost;
}
