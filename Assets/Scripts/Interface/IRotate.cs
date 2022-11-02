using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotate
{
    float TurnSpeed { get; }
    void RotateTower(Vector3 target);
}
