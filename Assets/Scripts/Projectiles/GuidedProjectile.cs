using UnityEngine;

public class GuidedProjectile : Projectile 
{
	private Vector3 m_targetPosition;

	private void Update()
	{
		if (m_targetPosition == null)
			Destroy(gameObject);

		if (m_targetPosition != null)
			Movement();
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

	private void OnTriggerEnter(Collider other) 
	{
		var monster = other.GetComponent<Monster>();

		if (monster == null)
			return;

		monster.TakeDamage(m_damage);
		Destroy(this.gameObject);
	}
}
