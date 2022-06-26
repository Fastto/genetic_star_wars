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

    [SerializeField] private GameObject[] guns;
    [SerializeField] private GameObject[] holds;
    [SerializeField] private SpriteRenderer armor;
    
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

    public int killedEnemies = 0;
    public int collectedGold = 0;
    public int unloadedGold = 0;
    public int damaged = 0;
    
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
    public Action<ShipController> OnEnemyKill;

    public Action<int> OnLivesChange;
    public Action<int> OnHoldChange;
    
    private void Start()
    {
        DisableEngine();
        
        SetColor(Team.color);
        ApplyGenome();

        Lives = Health;
        
        OnLivesChange?.Invoke(Lives);
        OnHoldChange?.Invoke(Hold);
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
        // Capacity 1 + 0..20
        // Damage = 1 + 0..20
        // Health = 4 + 0..80
        
        // G1 = 0 : Capacity = 1 + 0
        // G1 = .5 : Capacity = 1 + 10
        // G1 = 1 : Capacity = 1 + 20
        
        // G1 = 0, G2 = 0 : Damage = 1 + 0, Health = 4 + 80
        // G1 = 0, G2 = 1 : Damage = 1 + 20, Health = 4 + 0
        
        // G1 = .5, G2 = 0 : Damage = 1 + 0, Health = 4 + 40
        // G1 = .5, G2 = .5 : Damage = 1 + 5, Health = 4 + 20
        // G1 = .5, G2 = 1 : Damage = 1 + 10, Health = 4 + 0
        
        // G1 = 1, G2 = 0 : Damage = 1 + 0, Health = 4 + 0
        // G1 = 1, G2 = 1 : Damage = 1 + 0, Health = 4 + 0
        
        Capacity = Genome.GetCapacity();
        Damage = Genome.GetDamage();
        Health = Genome.GetHealth();

        var armorColor = armor.color;
        armorColor.a = Genome.GetHealthPoints();
        armor.color = armorColor;

        var gunScale = .7f + Genome.GetDamagePoints() * 3;
        foreach (var gun in guns)
        {
            gun.transform.localScale = new Vector3(gunScale, gunScale, 1);
        }
        
        var holdScale = .5f + Genome.GetCapacityPoints() * 2;
        foreach (var hold in holds)
        {
            hold.transform.localScale = new Vector3(holdScale, holdScale, 1);
        }
        
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

    private bool _isKilled = false;
    
    public bool MakeDamage(int damage)
    {
        if (damage > Lives)
            damage = Lives;

        Lives -= damage;
        OnLivesChange?.Invoke(Lives);

        bool isKilled = false;
        if (Lives == 0 && !_isKilled)
        {
            _isKilled = true;
            isKilled = true;
        }

        return isKilled;
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

    public int GetScore()
    {
        //return killedEnemies + collectedGold + unloadedGold + damaged;
        return killedEnemies * 20 + collectedGold + unloadedGold + damaged;
    }
}
