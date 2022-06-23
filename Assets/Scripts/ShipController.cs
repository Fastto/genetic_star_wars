using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer shipBodySpriteRenderer;
    [SerializeField] private ParticleSystem engineParticles;
    [SerializeField] private ParticleSystem[] gunParticles;
    
    [HideInInspector] public StationController Station;
    [HideInInspector] public ShipGenome Genome;
    [HideInInspector] public Team Team;
    
    
    //Genome managed properties
    public int Capacity;
    public int Health;
    public int Damage;
    
    //Predefined properties
    public float ShootCoolDown;
    public int MiningRate;
    public float MiningCoolDown;
    public int UnloadingRate;
    public float UnloadingCoolDown;
    public float RotationSpeed;
    public float Speed;
    
    //Other properties
    public int Hold;
    public int Lives;
    [HideInInspector] public bool IsConnectedToGold { get; private set; }
    [HideInInspector] public bool IsConnectedToStation { get; private set; }
    [HideInInspector] public bool IsConnectedToEnemy { get; private set; }
    [HideInInspector] public ShipController ConnectedEnemy { get; private set; }
    [HideInInspector] public GoldController ConnectedGold { get; private set; }
    [HideInInspector] public StationController ConnectedStation { get; private set; }

    public Action<ShipController> OnDie;
    public Action<ShipController> OnEnemyConnect;
    public Action<ShipController> OnEnemyDisconnect;
    
    private void Start()
    {
        DisableEngine();
        
        SetColor(Team.color);
        ApplyGenome();

        Lives = Health;
    }

    private void Update()
    {
        if (Lives <= 0)
        {
            OnDie?.Invoke(this);
            Destroy(gameObject);
        }
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
        shipController.OnDie += OnEnemyDieHandler;

        OnEnemyConnect?.Invoke(shipController);
    }

    private void DisconnectFromEnemy(ShipController shipController)
    {
        shipController.OnDie -= OnEnemyDieHandler;
        IsConnectedToEnemy = false;
        ConnectedEnemy = null;
        
        OnEnemyDisconnect?.Invoke(shipController);
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
        else if (!IsConnectedToStation && other.TryGetComponent(out StationController stationController))
        {
            ConnectToStation(stationController);
        }
        else if (!IsConnectedToEnemy 
                 && other.TryGetComponent(out ShipController shipController)
                 && shipController.Team != Team)
        {
            ConnectToEnemy(shipController);
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
        else if (IsConnectedToStation && other.TryGetComponent(out StationController stationController))
        {
            DisconnectFromStation(stationController);
        }
        else if (IsConnectedToEnemy 
                 && other.TryGetComponent(out ShipController shipController)
                 && ConnectedEnemy == shipController)
        {
            DisconnectFromEnemy(shipController);
        }
    }

    private void OnGoldOverHandler(GoldController goldController)
    {
        DisconnectFromGold(goldController);
    }
    
    private void OnEnemyDieHandler(ShipController shipController)
    {
        DisconnectFromEnemy(shipController);
    }

    public int MakeDamage(int damage)
    {
        if (damage > Lives)
            damage = Lives;

        Lives -= damage;

        return damage;
    }
    
    public void Move(Vector3 direction)
    {
        rigidbody.MovePosition(transform.position + direction * (Time.deltaTime * Speed));
    } 
    
    public void Rotate(Vector3 direction)
    {
        //TODO: _ship.rigidbody.MoveRotation();
        transform.up = Vector3.Lerp(transform.up, direction, RotationSpeed);
    }

    public void EnableEngine()
    {
        if(gameObject == null)
            return;
        
        engineParticles.Play();
    }
    
    public void DisableEngine()
    {
        if(gameObject == null)
            return;
        
        engineParticles.Stop();
    }

    public void Fire()
    {
        if(gameObject == null)
            return;
        
        foreach (var gunParticle in gunParticles)
        {
            gunParticle.Play();
        }
    }
}
