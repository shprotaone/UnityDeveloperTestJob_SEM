using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    private const int MonsterLayer = 1 << 12;

    [SerializeField] protected GameObject m_projectilePrefab;

    [SerializeField] protected Transform m_shootPoint;

    [SerializeField] protected float m_shootInterval;
    [SerializeField] protected float m_range;
    
    private float m_lastShotTime;

    protected Monster _targetMonster;

    protected void FindTarget()
    {
        Collider[] colliderArray = (Physics.OverlapSphere(transform.position, m_range, MonsterLayer));

        foreach (var collider in colliderArray)
        {
            if (collider.TryGetComponent(out Monster monster))
            {
                _targetMonster = monster;
            }
            else
            {
                _targetMonster = null;
            }
        }
    }

    protected void ShootDelay()
    {
        if (m_lastShotTime <= 0f)
        {
            Shoot(_targetMonster.gameObject);
            m_lastShotTime = 1f / m_shootInterval;
        }

        m_lastShotTime -= Time.deltaTime;
    }

    protected void ResetTarget()
    {
        if(_targetMonster != null)
        {
            if (Vector3.Distance(transform.position, _targetMonster.transform.position) > m_range)
            {
                _targetMonster = null;
            }
        }      
    }

    public abstract void Shoot(GameObject monster);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.localPosition;
        Gizmos.DrawWireSphere(position, m_range);

        if(_targetMonster != null)
        Gizmos.DrawLine(transform.position, _targetMonster.transform.position);
    }
}
