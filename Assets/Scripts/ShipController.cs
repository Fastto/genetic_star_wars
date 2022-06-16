using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer shipBodySpriteRenderer;
    
    [HideInInspector] public StationController Station;
    [HideInInspector] public ShipGenome Genome;
    [HideInInspector] public Team Team;
    
    //Genome managed properties
    public int Capacity { get; protected set; }
    public int Health { get; protected set; }
    public int Damage { get; protected set; }
    
    //Predefined properties
    public float ShootCoolDown;
    public int MiningRate;
    public float MiningCoolDown;
    public int UnloadingRate;
    public float UnloadingCoolDown;
    
    //Other properties
    [HideInInspector] public int Hold;
    [HideInInspector] public int Lives;
    [HideInInspector] public bool IsConnectedToGold { get; private set; }
    [HideInInspector] public bool IsConnectedToStation { get; private set; }
    [HideInInspector] public bool IsConnectedToEnemy { get; private set; }
    [HideInInspector] public ShipController ConnectedEnemy { get; private set; }
    [HideInInspector] public GoldController ConnectedGold { get; private set; }
    [HideInInspector] public StationController ConnectedStation { get; private set; }
    
    
    private void Start()
    {
        SetColor(Team.color);
        ApplyGenome();
    }

    public void SetColor(Color color)
    {
        shipBodySpriteRenderer.color = color;
    }

    private void ApplyGenome()
    {
        Capacity = 10;
        Damage = 5;
        Health = 20;
    }

    private void ConnectToGold(GoldController goldController)
    {
        if (goldController.TryToConnect(this))
        {
            IsConnectedToGold = true;
            ConnectedGold = goldController;
            goldController.OnOver += OnGoldOverHandler;
        }
    }

    private void DisconnectFromGold(GoldController goldController)
    {
        goldController.OnOver -= OnGoldOverHandler;
        goldController.Disconnect(this);
        IsConnectedToGold = false;
        ConnectedGold = null;
    }
    
    private void ConnectToStation(StationController stationController)
    {
        IsConnectedToStation = true;
        ConnectedStation = stationController;
    }

    private void DisconnectFromStation(StationController stationController)
    {
        IsConnectedToStation = false;
        ConnectedStation = null;
    }
    
    private void ConnectToEnemy(ShipController shipController)
    {
        IsConnectedToEnemy = true;
        ConnectedEnemy = shipController;
    }

    private void DisconnectFromEnemy(ShipController shipController)
    {
        IsConnectedToEnemy = false;
        ConnectedEnemy = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsConnectedToGold 
            && !IsConnectedToEnemy
            && other.TryGetComponent(out GoldController goldController) 
            && goldController.IsFree)
        {
            ConnectToGold(goldController);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsConnectedToGold 
            && other.TryGetComponent(out GoldController goldController) 
            && goldController == ConnectedGold)
        {
            DisconnectFromGold(goldController);
        }
    }

    private void OnGoldOverHandler(GoldController goldController)
    {
        DisconnectFromGold(goldController);
    }
}
