using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    [SerializeField] private int initialAmount;
    private int _amount;

    public Action<GoldController> OnOver;
    public Action<GoldController> OnConnect;
    public Action<GoldController> OnDisconnect;
    
    public bool IsFree { get; private set; } = true;
    public ShipController ConnectedShip { get; private set; }

    private void Start()
    {
        _amount = initialAmount;
    }

    public bool TryToConnect(ShipController shipController)
    {
        bool success = false;
        if (IsFree)
        {
            IsFree = false;
            ConnectedShip = shipController;
            success = true;
            OnConnect?.Invoke(this);
        }

        return success;
    }

    public void Disconnect(ShipController shipController)
    {
        if (!IsFree && shipController == ConnectedShip)
        {
            IsFree = true;
            ConnectedShip = null;
            OnDisconnect?.Invoke(this);
        }
    }

    public int GetGold(int quantity)
    {
        if (_amount == 0)
        {
            OnOver?.Invoke(this);
            Destroy(this.gameObject);
        }
        
        if (quantity > _amount)
            quantity = _amount;

        _amount -= quantity;
        return _amount;
    }
}
