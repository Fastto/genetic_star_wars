using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
   [SerializeField] public ResourcesManager ResourcesManager;
   [SerializeField] private Team team;

   [SerializeField] private GameObject shipPrefab;
   [SerializeField] private ShipGenome shipInitialGenome;

   [SerializeField] private int initialGold;
   [SerializeField] private int shipCost;

   [SerializeField] private float shipBuildingTime;
   
   private int _gold;
   
   private void Start()
   {
      _gold = initialGold;
      StartCoroutine(WaitForResourcesCoroutine());
   }

   private IEnumerator BuildTheShipCoroutine()
   {
      _gold -= shipCost;

      yield return new WaitForSeconds(shipBuildingTime);

      BuildTheShip();
      
      StartCoroutine(WaitForResourcesCoroutine());
   }
   
   private IEnumerator WaitForResourcesCoroutine()
   {
      while (_gold < shipCost)
      {
         yield return null;
      }

      StartCoroutine(BuildTheShipCoroutine());
   }

   private void BuildTheShip()
   {
      GameObject shipGO = Instantiate(shipPrefab, transform.position, Quaternion.identity);
      ShipController shipController = shipGO.GetComponent<ShipController>();
      shipController.Station = this;
      shipController.Team = team;
      shipController.Genome = shipInitialGenome;
   }
}
