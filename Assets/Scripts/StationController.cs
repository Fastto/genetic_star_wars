using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
   [SerializeField] private SpriteRenderer stationBodyRenderer;
   [SerializeField] private ParticleSystem unloadingParticles;
   
   
   [SerializeField] public ResourcesManager ResourcesManager;
   [SerializeField] public Team team;

   [SerializeField] private GameObject shipPrefab;

   [SerializeField] private int initialGold;
   [SerializeField] private int shipCost;

   [SerializeField] private float shipBuildingTime;

   [SerializeField] private float idleRotationSpeed;
   [SerializeField] private float shipProduceRotationSpeed;

   [SerializeField] public Strategy strategy;
   private Strategy _strategy;
   
   private float _rotationSpeed;
   
   private int _gold;

   public Action<int> OnGoldCollected;
   public Action<int> OnGoldChange;
   public Action<ShipController> OnShipProduce;
   public Action<ShipController> OnShipDie;
   public Action<ShipController> OnEnemyDie;
   
   private void Start()
   {
      stationBodyRenderer.color = team.color;
      
      _rotationSpeed = idleRotationSpeed;
      
      _gold = initialGold;
      StartCoroutine(WaitForResourcesCoroutine());

      _strategy = Instantiate(strategy);
   }

   private void Update()
   {
      transform.Rotate(Vector3.forward, _rotationSpeed);
   }

   private IEnumerator BuildTheShipCoroutine()
   {
      _rotationSpeed = shipProduceRotationSpeed;
      
      _gold -= shipCost;
      OnGoldChange?.Invoke(_gold);
      
      yield return new WaitForSeconds(shipBuildingTime);

      BuildTheShip();
      
      StartCoroutine(WaitForResourcesCoroutine());
   }
   
   private IEnumerator WaitForResourcesCoroutine()
   {
      _rotationSpeed = idleRotationSpeed;
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
      shipController.Genome = _strategy.GetGenome();
      shipController.OnDie += OnShipDieHandler;
      shipController.OnEnemyKill += controller => { OnEnemyDie?.Invoke(controller);}; 
      
      OnShipProduce?.Invoke(shipController);
      ResourcesManager.RegisterShip(shipController);
      _strategy.RegisterShip(shipController);
   }

   public void PutGold(int amount)
   {
      _gold += amount;
      unloadingParticles.Play();
      
      OnGoldCollected?.Invoke(amount);
      OnGoldChange?.Invoke(_gold);
   }

   private void OnShipDieHandler(ShipController shipController)
   {
      OnShipDie?.Invoke(shipController);
      
      var goldAmount = shipController.Hold;
      if (goldAmount > 0)
      {
         ResourcesManager.DropGold(shipController.transform.position, goldAmount);
      }
   }
}
