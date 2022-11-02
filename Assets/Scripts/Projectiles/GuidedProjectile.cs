using UnityEngine;

public class GuidedProjectile : Projectile,IPooledObject
{
	private Vector3 m_targetPosition;
	private ObjectType m_objectType = ObjectType.GUIDEDPROJ;

    public ObjectType Type => m_objectType;

    private void FixedUpdate()
	{
		if (m_targetPosition == null)
			DisableProjectile();

		if (m_targetPosition != null)
			Movement();
	}
	private void OnTriggerEnter(Collider other)
	{
		var monster = other.GetComponent<Monster>();

		if (monster != null)
		{
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
		m_targetPosition = target;
    }

	public override void Movement()
    {
		var translation = m_targetPosition - transform.position;

		if (translation.magnitude > m_speed)
		{
			translation = translation.normalized * m_speed;
		}
		transform.Translate(translation);
	}

	public override void DisableProjectile()
    {
		ObjectPool.SharedInstance.DestroyObject(gameObject);
	}
}
