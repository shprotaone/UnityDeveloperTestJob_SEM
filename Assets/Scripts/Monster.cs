using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour 
{
	private Action OnCheckDeath;
	private const float m_reachDistance = 0.3f;

	[SerializeField] private float m_speed = 0.1f;
	[SerializeField] private int m_maxHP = 30;

	private GameObject m_moveTarget;
	private Vector3 m_lastPosition;
	
	private int m_hp;

    public Vector3 Velocity { get; set; }

	private void Start() 
	{
		m_hp = m_maxHP;
		OnCheckDeath += Death;

		StartCoroutine(CalculateVelocity());
	}

	private void Update () 
	{
        if (m_moveTarget == null)
            return;

        Movement();
		DisableMonster();        
    }

	private void Movement()
    {
		var translation = m_moveTarget.transform.position - transform.position;

		transform.position += translation.normalized * m_speed * Time.deltaTime;
	}

	
	private void DisableMonster()
    {
		bool reach = Vector3.Distance(transform.position, m_moveTarget.transform.position) <= m_reachDistance;

		if (reach)
        {
			Destroy(gameObject);
			return;
        }		
    }

	public void SetMoveTarget(GameObject target)
    {
		m_moveTarget = target;		
	}

	private IEnumerator CalculateVelocity()
	{
        while (true)
        {
			m_lastPosition = transform.position;

			yield return new WaitForFixedUpdate();

			Velocity = (transform.position - m_lastPosition) / Time.deltaTime;
		}	
	}

	public void TakeDamage(int damage)
    {
		m_hp -= damage;
		OnCheckDeath?.Invoke();
    }

	private void Death()
    {
        if (m_hp <= 0)
        {
			Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
		OnCheckDeath -= Death;
    }
}
