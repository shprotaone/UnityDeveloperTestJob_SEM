using UnityEngine;

public class SimpleTower : Tower, IFindTarget, IShoot
{
    [SerializeField] private Transform m_shootPoint;
    [SerializeField] private Transform m_cannon;

    private float m_lastShotTime;

    public Monster Monster => m_targetMonster;
    public float ShootInterval => m_shootInterval;
    public float Range => m_range;

    private void Update () {

        ResetTarget();

        if (m_targetMonster == null)
        {
            FindTarget();
        }
        else
        {
            Shoot(m_targetMonster.transform.position);
        }
    }

    private void InitProjectile()
    {
        var projectile = ObjectPool.SharedInstance.GetObject(ObjectType.GUIDEDPROJ);
        projectile.transform.position = m_shootPoint.position;

        var projectileBeh = projectile.GetComponent<GuidedProjectile>();

        projectileBeh.SetTarget(m_targetMonster.transform.position);
    }

    public void Shoot(Vector3 target)
    {
        if (m_lastShotTime <= 0f)
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
}
