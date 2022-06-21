using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FollowToFreeGoldState : State
{
    protected GoldController _target;
    
    protected override void Init()
    {
        var list = _ship.Station.ResourcesManager.GetFree();

        if (list.Count > 0)
        {
            float distance = (list[0].transform.position - _ship.transform.position).magnitude;
            GoldController target = list[0];
            
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
            _target.OnConnect += OnGoldConnectHandler;
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

    protected void OnGoldConnectHandler(GoldController goldController)
    {
        goldController.OnConnect -= OnGoldConnectHandler;
        Finish();
    }
}
