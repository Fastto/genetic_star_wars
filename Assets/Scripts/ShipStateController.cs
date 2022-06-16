using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStateController : MonoBehaviour
{
    [SerializeField] private State idleState;
    [SerializeField] private State battleState;
    [SerializeField] private State miningState;
    [SerializeField] private State unloadingState;
    [SerializeField] private State followToBusyGoldState;
    [SerializeField] private State followToFreeGoldState;
    [SerializeField] private State followToEnemyState;
    [SerializeField] private State followToStationState;
    
    private ShipController _ship;
    private State _state;

    private void Start()
    {
        _ship = GetComponent<ShipController>();
        SetState(DetermineState());
    }

    private void Update()
    {
        if (_state.IsFinished)
        {
            SetState(DetermineState());
        }
        else
        {
            _state.Update();
        }
    }

    private void SetState(State state)
    {
        _state = Instantiate(state);
        _state.Init(_ship);
    }

    private State DetermineState()
    {
        if (_ship.Hold == 0 && !IsConnectedToGold && _ship.Station.ResourcesManager.HasFree)
        {
            return followToFreeGoldState;
        } 
        else if (_ship.Hold == 0 && !IsConnectedToGold &&  _ship.Station.ResourcesManager.HasCapturedByEnemy)
        {
            return followToBusyGoldState;
        } 
        else if (_ship.Hold == 0 && !IsConnectedToGold &&  !_ship.Station.ResourcesManager.HasCapturedByEnemy)
        {
            return followToEnemyState;
        } 
        else if (_ship.Hold == 0 && IsConnectedToGold)
        {
            return miningState;
        }
        else if (_ship.Hold > 0 && !IsConnectedToStation)
        {
            return followToStationState;
        }
        else if (_ship.Hold > 0 && IsConnectedToStation)
        {
            return unloadingState;
        } 
        else if (IsConnectedToEnemy)
        {
            return battleState;
        }

        return idleState;
    }
}
