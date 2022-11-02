using UnityEngine;

public class CannonTower : Tower, IFindTarget, IRotate, IShoot
{
    private const float LockVelocity = 1600;

    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private Transform m_cannon;
    [SerializeField] private float m_turnSpeed;
    
    private Vector3 m_lastAngle;
    private Vector3 m_velocityRotation;
    private Vector3 m_advance;

    private float m_lastShotTime;
    private bool m_targetIsLocked = true;

    public float ShootInterval => m_shootInterval;
    public float Range => m_range;
    public Monster Monster => m_targetMonster;
    public float TurnSpeed => m_turnSpeed;

    private void FixedUpdate () 
    {
        ResetTarget();

        if (m_targetMonster == null)
        {
            FindTarget();
        }
        else
        {
            RotateTower(m_targetMonster.transform.position);            
            Shoot(m_targetMonster.transform.position);
        }
    }

    private void InitProjectile()
    {
        var projectile = ObjectPool.SharedInstance.GetObject(ObjectType.CANNONPROJ);
        var projectileBeh = projectile.GetComponent<CannonProjectile>();
        projectile.transform.position = m_shootPoint.transform.position;

        m_advance = CalculateAdvance(projectileBeh.Speed);

        projectileBeh.SetTarget(m_advance);
    }

    public void Shoot(Vector3 target)
    {
        if (m_lastShotTime <= 0f && m_targetIsLocked)
        {
            InitProjectile();
            m_lastShotTime = 1f / ShootInterval;
        }

        m_lastShotTime -= Time.deltaTime;
    }

    public void FindTarget()
    {
        Collider[] colliderArray = (Physics.OverlapSphere(transform.position, Range, MonsterLayer));

        foreach (var collider in colliderArray)
        {
            if (collider.TryGetComponent(out Monster monster))
            {
                m_targetMonster = monster;
            }
            else
            {
                m_targetMonster = null;
            }
        };
    }

    public void RotateTower(Vector3 target)
    {
        CalculateVelocityRotation();

        Vector3 dir = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(m_cannon.rotation, lookRotation, Time.deltaTime * TurnSpeed).eulerAngles;

        m_cannon.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void CalculateVelocityRotation()
    {          
        m_velocityRotation = (m_cannon.rotation.eulerAngles - m_lastAngle) / Time.deltaTime;

        if (m_velocityRotation.sqrMagnitude < LockVelocity && m_velocityRotation.sqrMagnitude != 0)
        {
            m_targetIsLocked = true;
        }
        else
        {
            m_targetIsLocked = false;
        }

        m_lastAngle = m_cannon.rotation.eulerAngles;
    }

    private Vector3 CalculateAdvance(float projVelocity)
    {
        float distanceToTarget = Vector3.Distance(m_targetMonster.transform.position,transform.position);

        float timeToTarget = distanceToTarget / projVelocity;

        return m_targetMonster.transform.position + m_targetMonster.Velocity * timeToTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(m_advance,0.5f);
    }  
}
