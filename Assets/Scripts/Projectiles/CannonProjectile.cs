using UnityEngine;

public class CannonProjectile : Projectile,IPooledObject
{
	private Vector3 m_shootDirection;
    private Vector3 m_targetPosition;

    public float Speed => m_speed;

    public ObjectType Type => ObjectType.CANNONPROJ;

    private void FixedUpdate()
    {
        if (m_targetPosition != null)
            Movement();
    }

    private void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log("HIT");
            monster.TakeDamage(m_damage);
            DisableProjectile();
        }
        else
        {
            DisableProjectile();
        }
    }

    public override void SetTarget(Vector3 target)
	{
        m_shootDirection = target - transform.position;
	}

	public override void Movement()
    {
        transform.position += m_shootDirection.normalized * m_speed * Time.deltaTime;
    }

    public override void DisableProjectile()
    {
        ObjectPool.SharedInstance.DestroyObject(gameObject);
    }
}
