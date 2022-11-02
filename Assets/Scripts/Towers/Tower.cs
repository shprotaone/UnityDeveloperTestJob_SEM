using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected const int MonsterLayer = 1 << 12;

    [SerializeField] protected float m_shootInterval;
    [SerializeField] protected float m_range;
    
    protected Monster m_targetMonster;

    protected void ResetTarget()
    {
        if(m_targetMonster != null)
        {
            if (Vector3.Distance(transform.position, m_targetMonster.transform.position) >= m_range)
            {
                m_targetMonster = null;
            }
        }      
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.localPosition;
        Gizmos.DrawWireSphere(position, m_range);

        if(m_targetMonster != null)
        Gizmos.DrawLine(transform.position, m_targetMonster.transform.position);
    }
}
