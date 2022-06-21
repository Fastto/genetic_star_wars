using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattleState : State
{
    private ShipController _enemy;
    
    protected override void Init()
    {
        if (_ship.IsConnectedToEnemy)
        {
            _enemy = _ship.ConnectedEnemy;
            _ship.OnEnemyDisconnect += OnEnemyDisconnectHandler;

            _ship.StartCoroutine(Battle());
        }
        else
        {
            Finish();
        }
    }

    IEnumerator Battle()
    {
        while (!IsFinished && _ship.IsConnectedToEnemy)
        {
            yield return new WaitForSeconds(_ship.ShootCoolDown);
            _enemy.MakeDamage(_ship.Damage);
        }
    }
    
    protected override void Run()
    {
        Vector3 direction = (_enemy.transform.position - _ship.transform.position).normalized;
        _ship.Rotate(direction);
    }

    private void OnEnemyDisconnectHandler(ShipController shipController)
    {
        _ship.OnEnemyDisconnect -= OnEnemyDisconnectHandler;
        Finish();
    }
}
