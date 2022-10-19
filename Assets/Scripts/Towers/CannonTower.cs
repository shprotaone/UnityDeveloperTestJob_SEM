using UnityEngine;
using System.Collections;

public class CannonTower : Tower 
{
    [SerializeField] private Transform m_cannon;

    [SerializeField] private float m_turnSpeed;

    private CannonProjectile m_cannonProjectie;

    private Vector3 m_lastAngle;
    private Vector3 m_velocityRotation;
    private Vector3 _advance;

    private bool m_targetIsLocked = true;

    private void Start()
    {
        StartCoroutine(CalculateVelocityRotation());
        m_cannonProjectie = m_projectilePrefab.GetComponent<CannonProjectile>();
    }

    private void FixedUpdate () 
    {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        ResetTarget();

        if(_targetMonster == null)
        {          
            FindTarget();           
        }
        else
        {
            RotateTower();
            if (m_targetIsLocked)
            ShootDelay();
        }    
	}

	private void RotateTower()
    {
        Vector3 dir = _targetMonster.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(m_cannon.rotation, lookRotation, Time.deltaTime * m_turnSpeed).eulerAngles;

        m_cannon.rotation = Quaternion.Euler(0f ,rotation.y ,0f );
        
    }

    private IEnumerator CalculateVelocityRotation()
    {
        while (true)
        {
            m_lastAngle = m_cannon.rotation.eulerAngles;

            yield return new WaitForFixedUpdate();

            m_velocityRotation = (m_cannon.rotation.eulerAngles - m_lastAngle) / Time.deltaTime;

            if (m_velocityRotation.magnitude < 40 && m_velocityRotation.magnitude != 0)
            {
                m_targetIsLocked = true;
            }
            else
            {
                m_targetIsLocked = false;
            }

            yield return null;
        }       
    }

	public override void Shoot(GameObject monster)
	{       
        var projectile = Instantiate(m_projectilePrefab, m_shootPoint.transform.position, Quaternion.identity) as GameObject;
        var projectileBeh = projectile.GetComponent<CannonProjectile>();

        _advance = CalculateAdvance(_targetMonster, m_cannonProjectie.Speed);

        projectileBeh.SetTarget(_advance);
    }

    private Vector3 CalculateAdvance(Monster target, float projVelocity)
    {
        float distanceToTarget = Vector3.Distance(target.transform.position,transform.position);

        float timeToTarget = distanceToTarget / projVelocity;

        return target.transform.position + target.Velocity * timeToTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_advance,0.5f);
    }

}
