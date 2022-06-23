using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IdleState : State
{
    [SerializeField] private float delay;
    protected override void Init()
    {
        //_ship.StartCoroutine(Wait());
        
        _ship.EnableEngine();
    }
    
    protected override void Run()
    {
        Vector3 direction = _ship.transform.up;
        _ship.Move(direction);
        _ship.Rotate(direction);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        Finish();
    }
    
    public override void Finish()
    {
        if(_ship != null)
            _ship.DisableEngine();
        
        base.Finish();
    }
}
