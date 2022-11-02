using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    float ShootInterval { get; }
    void Shoot(Vector3 target);
}
