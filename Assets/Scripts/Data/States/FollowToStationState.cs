using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FollowToStationState : FollowToState
{
    protected StationController _target;
    
    protected override void Init()
    {
        if (!_ship.IsConnectedToStation)
        {
            _target = _ship.Station;
        }
        else
        {
            Finish();
        }
    }
    
    protected override void Run()
    {
        Vector3 direction = (_target.transform.position - _ship.transform.position).normalized;
        Move(direction);
    }
    
    protected override bool ConditionToFinish()
    {
        return _ship.IsConnectedToStation;
    }
}
