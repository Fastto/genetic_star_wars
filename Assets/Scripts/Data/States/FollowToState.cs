using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowToState : State
{
    protected void Move(Vector3 direction)
    {
        _ship.rigidbody.MovePosition(_ship.transform.position + direction * (Time.deltaTime * _ship.Speed));
        
        //TODO: _ship.rigidbody.MoveRotation();
        _ship.transform.up = Vector3.Lerp(_ship.transform.up, direction, _ship.RotationSpeed);
    }
}
