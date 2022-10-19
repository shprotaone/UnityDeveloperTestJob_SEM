using UnityEngine;

public class SimpleTower : Tower 
{
    private void Update () {
        if (m_projectilePrefab == null)
            return;

        ResetTarget();

        if(_targetMonster == null)
        {
            FindTarget();
        }
        else
        {
            ShootDelay();
        }       
	}

    public override void Shoot(GameObject monster)
    {
        var projectile = Instantiate(m_projectilePrefab, m_shootPoint);
        var projectileBeh = projectile.GetComponent<GuidedProjectile>();

        projectileBeh.SetTarget(monster.transform.position);
    }
}
