using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFindTarget
{
    float Range { get; }
    Monster Monster { get; }
    void FindTarget();
}
