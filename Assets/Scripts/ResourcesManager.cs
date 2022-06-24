using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private GameObject goldPrefab;
    
    private List<GoldController> _goldList;
    private List<ShipController> _shipList;

    public bool HasFree()
    {
        return GetFree().Count > 0;
    }

    public bool HasCapturedByEnemy(Team requestorTeam)
    {
        return GetCapturedByEnemy(requestorTeam).Count > 0;
    }
    
    public bool HasEnemies(Team requestorTeam)
    {
        return GetEnemies(requestorTeam).Count > 0;
    }


    private void Start()
    {
        _shipList = new List<ShipController>();
        _goldList = new List<GoldController>();
        
        
        var goldArr = FindObjectsOfType<GoldController>();
        foreach (var goldController in goldArr)
        {
            RegisterGold(goldController);
        }
    }

    public void RegisterGold(GoldController goldController)
    {
        goldController.OnOver += controller =>
        {
            if (_goldList.Contains(controller))
                _goldList.Remove(controller);
        };

        _goldList.Add(goldController);
    }
    
    public void RegisterShip(ShipController shipController)
    {
        shipController.OnDie += controller =>
        {
            if (_shipList.Contains(controller))
            {
                _shipList.Remove(controller);
            }
        };

        _shipList.Add(shipController);
        
    }

    public List<GoldController> GetFree()
    {
        var freeList = new List<GoldController>();

        _goldList.ForEach(item =>
        {
            if (item.IsFree)
                freeList.Add(item);
        });

        return freeList;
    }

    public List<GoldController> GetCapturedByEnemy(Team requestorTeam)
    {
        var list = new List<GoldController>();

        _goldList.ForEach(item =>
        {
            if (!item.IsFree && item.ConnectedShip.Team != requestorTeam)
                list.Add(item);
        });

        return list;
    }

    public void DropGold(Vector3 position, int goldAmount)
    {
        var goldGO = Instantiate(goldPrefab, position, Quaternion.identity);
        var goldController = goldGO.GetComponent<GoldController>();
        goldController.SetInitialAmount(goldAmount);
        
        RegisterGold(goldController);
    }
    
    public List<ShipController> GetEnemies(Team requestorTeam)
    {
        var list = new List<ShipController>();

        _shipList.ForEach(item =>
        {
            if (item.Team != requestorTeam)
                list.Add(item);
        });

        return list;
    }
}