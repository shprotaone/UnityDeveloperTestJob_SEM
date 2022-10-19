using UnityEngine;

public class CannonProjectile : Projectile 
{
	private Vector3 m_shootDirection;

    private Vector3 m_targetPosition;
    public float Speed => m_speed;

    private void Update()
    {
        if (m_targetPosition != null)
            Movement();
    }


    public override void SetTarget(Vector3 target)
	{
        m_shootDirection = target - transform.position;
	}

	public override void Movement()
    {
        transform.position += m_shootDirection.normalized * m_speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var monster = other.GetComponent<Monster>();

        if (monster != null)
        {
            Debug.Log("HIT");
            monster.TakeDamage(m_damage);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }       
    }
}
