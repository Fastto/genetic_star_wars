using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStatistics : MonoBehaviour
{
    [SerializeField] private ShipController ship;
    [SerializeField] private Slider health;
    [SerializeField] private Slider hold;

    Quaternion rotation;
    private Vector3 position;
    
    void Awake()
    {
        rotation = transform.rotation;
        position = transform.position;
    }
    
    void LateUpdate()
    {
        transform.rotation = rotation;
        transform.position = ship.transform.position + Vector3.up * .6f;
    }
    
    private void OnEnable()
    {
        ship.OnLivesChange += OnLivesChangeHandler;
        ship.OnHoldChange += OnHoldChangeHandler;
    }
    
    private void OnDisable()
    {
        ship.OnLivesChange -= OnLivesChangeHandler;
        ship.OnHoldChange -= OnHoldChangeHandler;
    }
    

    private void OnLivesChangeHandler(int amount)
    {
        health.value = amount;
    }
    
    private void OnHoldChangeHandler(int amount)
    {
        hold.value = amount;
    }

    private void Start()
    { 
        // health.maxValue = ship.Health;
        // hold.maxValue = ship.Capacity;
    }
}
