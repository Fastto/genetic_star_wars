using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDemo : MonoBehaviour
{
    private ShipController ship;

    public float engineEnabledTime;
    public float engineDisabledTime;
    
    public float fireEnabledTime;
    public float fireDisabledTime;
    public float fireBetweenShootsTime;
    
    void Start()
    {
        ship = GetComponent<ShipController>();
        
        StartCoroutine(EnableEngine());
        StartCoroutine(EnableFire());
    }

    IEnumerator EnableEngine()
    {
        ship.EnableEngine();
        yield return new WaitForSeconds(engineEnabledTime);

        StartCoroutine(DisableEngine());
    }
    
    IEnumerator DisableEngine()
    {
        ship.DisableEngine();
        yield return new WaitForSeconds(engineDisabledTime);
        
        StartCoroutine(EnableEngine());
    }

    private float _fireStarted = 0;
    IEnumerator EnableFire()
    {
        _fireStarted = Time.time;
        bool isLeft = true;

        while (Time.time - _fireStarted < fireEnabledTime)
        {
            if (isLeft)
                ship.LeftFire();
            else 
                ship.RightFire();
            
            isLeft = !isLeft;
            yield return new WaitForSeconds(fireBetweenShootsTime);
        }
        StartCoroutine(DisableFire());
    }
    
    IEnumerator DisableFire()
    {
        yield return new WaitForSeconds(fireDisabledTime);
        StartCoroutine(EnableFire());
    }
}
