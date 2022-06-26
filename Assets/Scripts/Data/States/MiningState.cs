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
            _gold.OnOver += OnGoldDisconnectHandler;

            _ship.StartCoroutine(Mining());
        }
        else
        {
            Finish();
        }
    }

    IEnumerator Mining()
    {
        while (!IsFinished && _ship.Hold < _ship.Capacity && !_gold.IsEmpty())
        {
            yield return new WaitForSeconds(_ship.MiningCoolDown);
            
            int amount = _ship.MiningRate;
            
            if (amount > _ship.Capacity - _ship.Hold)
                amount = _ship.Capacity - _ship.Hold;
            
            int gold = _gold.GetGold(amount);
            _ship.Hold += gold;
            _ship.collectedGold += gold;
            
            _ship.OnHoldChange?.Invoke(_ship.Hold);
        }
        
        Finish();
    }
    
    protected override void Run()
    {
        Vector3 direction = (_gold.transform.position - _ship.transform.position).normalized;
        _ship.Rotate(direction);
    }

    private void OnGoldDisconnectHandler(GoldController goldController)
    {
        _gold.OnDisconnect -= OnGoldDisconnectHandler;
        _gold.OnOver -= OnGoldDisconnectHandler;
        Finish();
    }
}
