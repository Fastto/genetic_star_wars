using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FollowToEnemyState : State
{
    protected ShipController _target;
    
    protected override void Init()
    {
        var list = _ship.Station.ResourcesManager.GetEnemies(_ship.Team);

        if (list.Count > 0)
        {
            float distance = (list[0].transform.position - _ship.transform.position).magnitude;
            ShipController target = list[0];
            
            list.ForEach(item =>
            {
                float distanceToItem = (item.transform.position - _ship.transform.position).magnitude;
                if (distanceToItem < distance)
                {
                    distance = distanceToItem;
                    target = item;
                }
            });

            _target = target;
            _target.OnDie += OnEnemyDieHandler;
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

    protected void OnEnemyDieHandler(ShipController shipController)
    {
        shipController.OnDie -= OnEnemyDieHandler;
        Finish();
    }
}
