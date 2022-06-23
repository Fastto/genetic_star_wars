using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FollowToStationState : State
{
    protected StationController _target;
    
    protected override void Init()
    {
        if (!_ship.IsConnectedToStation)
        {
            _target = _ship.Station;
            
            _ship.EnableEngine();
        }
        else
        {
            Finish();
        }
    }
    
    protected override void Run()
    {
        Vector3 direction = (_target.transform.position - _ship.transform.position).normalized;
        _ship.Move(direction);
        _ship.Rotate(direction);
    }
    
    protected override bool ConditionToFinish()
    {
        return _ship.IsConnectedToStation;
    }
    
    public override void Finish()
    {
        if(_ship != null)
            _ship.DisableEngine();
        
        base.Finish();
    }
}
