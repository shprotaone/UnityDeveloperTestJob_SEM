using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Monster : MonoBehaviour,IPooledObject
{
	private const float m_reachDistance = 0.3f;

	[SerializeField] private float m_speed = 0.1f;
	[SerializeField] private int m_maxHP = 30;

	private GameObject m_moveTarget;
	private Vector3 m_lastPosition;
	private ObjectType m_type = ObjectType.MONSTER;
	
	private int m_hp;

    public Vector3 Velocity { get; set; }

    public ObjectType Type => m_type;

    private void Start() 
	{
		m_hp = m_maxHP;
	}

	private void FixedUpdate () 
	{
        if (m_moveTarget == null)
            return;

		CalculateVelocity();
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
			ObjectPool.SharedInstance.DestroyObject(gameObject);
			return;
        }		
    }

	public void SetMoveTarget(GameObject target)
    {
		m_moveTarget = target;		
	}

	private void CalculateVelocity()
	{	
		Velocity = (transform.position - m_lastPosition) / Time.deltaTime;

		m_lastPosition = transform.position;
	}

	public void TakeDamage(int damage)
    {
		m_hp -= damage;
		Death();
    }

	private void Death()
    {
        if (m_hp <= 0)
        {
			Destroy(gameObject);
        }
    }
}
