using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
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
    public int Hold { get; set; }
    public int Lives { get; set; }
    
    
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
}
