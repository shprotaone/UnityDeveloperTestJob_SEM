using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_speed;
    [SerializeField] protected int m_damage;
  
    public abstract void Movement();
    public abstract void SetTarget(Vector3 target);
}
