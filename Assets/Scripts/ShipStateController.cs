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
        State state = idleState;
        
        if (_ship.Hold == 0 
            && !_ship.IsConnectedToGold 
            && _ship.Station.ResourcesManager.HasFree())
        {
            state = followToFreeGoldState;
        } 
        else if (_ship.Hold == 0 
                 && !_ship.IsConnectedToGold 
                 &&  _ship.Station.ResourcesManager.HasCapturedByEnemy(_ship.Team))
        {
            state = followToBusyGoldState;
        } 
        else if (_ship.Hold == 0 
                 && !_ship.IsConnectedToGold 
                 &&  !_ship.Station.ResourcesManager.HasCapturedByEnemy(_ship.Team))
        {
            state = followToEnemyState;
        } 
        else if (_ship.Hold == 0 && _ship.IsConnectedToGold)
        {
            state = miningState;
        }
        else if (_ship.Hold > 0 && !_ship.IsConnectedToStation)
        {
            state = followToStationState;
        }
        else if (_ship.Hold > 0 && _ship.IsConnectedToStation)
        {
            state = unloadingState;
        } 
        else if (_ship.IsConnectedToEnemy)
        {
            state = battleState;
        }

        return state;
    }
}
