using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MiningState : State
{
    private GoldController _gold;
    
    protected override void Init()
    {
        if (_ship.IsConnectedToGold)
        {
            _gold = _ship.ConnectedGold;
            _gold.OnDisconnect += OnGoldDisconnectHandler;

            _ship.StartCoroutine(Mining());
        }
        else
        {
            Finish();
        }
    }

    IEnumerator Mining()
    {
        while (!IsFinished && _ship.Hold < _ship.Capacity)
        {
            yield return new WaitForSeconds(_ship.MiningCoolDown);
            
            int amount = _ship.MiningRate;
            
            if (amount > _ship.Capacity - _ship.Hold)
                amount = _ship.Capacity - _ship.Hold;
            
            _ship.Hold += _gold.GetGold(amount);
        }
        
        Finish();
    }

    private void OnGoldDisconnectHandler(GoldController goldController)
    {
        _gold.OnDisconnect -= OnGoldDisconnectHandler;
        Finish();
    }
}
