using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    [SerializeField] private int initialAmount;

    private int _amount;
    
    public int Amount
    {
        get
        {
            return _amount;
        }
        set
        {
            _amount = value;
            if (value > 0)
            {
                float a = value / 30f;
                a = a == 0 ? 1 : a;
                float scale = Mathf.Sqrt(a / Mathf.PI);
                scale = scale > 1 ? 1 : scale;
                scale = scale < 0.2f ? .2f : scale;

                transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

    public Action<GoldController> OnOver;
    public Action<GoldController> OnConnect;
    public Action<GoldController> OnDisconnect;
    
    public bool IsFree { get; private set; } = true;
    public ShipController ConnectedShip { get; private set; }

    private void Start()
    {
        Amount = initialAmount;
    }

    private void Update()
    {
        if (Amount <= 0)
        {
            OnOver?.Invoke(this);
            Destroy(this.gameObject);
        }
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
        if (quantity > Amount)
            quantity = Amount;

        Amount -= quantity;
        return quantity;
    }

    public void SetInitialAmount(int amount)
    {
        initialAmount = amount;
    }
}
