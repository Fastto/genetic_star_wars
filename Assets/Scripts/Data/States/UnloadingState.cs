using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnloadingState : State
{
    private StationController _station;
    
    protected override void Init()
    {
        if (_ship.IsConnectedToStation)
        {
            _station = _ship.ConnectedStation;

            _ship.StartCoroutine(Unloading());
        }
        else
        {
            Finish();
        }
    }

    IEnumerator Unloading()
    {
        while (!IsFinished && _ship.Hold > 0)
        {
            yield return new WaitForSeconds(_ship.UnloadingCoolDown);
            
            int amount = _ship.UnloadingRate;
            
            if (amount > _ship.Hold)
                amount = _ship.Hold;
            
            _ship.Hold -= amount;
            _station.PutGold(amount);
        }
        
        Finish();
    }
    
}
