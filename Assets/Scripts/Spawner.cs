using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Transform m_moveTarget;
	[SerializeField] private GameObject m_monsterPrefab;

	[SerializeField] private float m_interval = 3;
	[SerializeField] private float m_lastSpawn = -1;

    private void Start()
    {
		CreateMonster();
    }

    private void Update() 
	{
        if (Time.time > m_lastSpawn + m_interval)
        {
            CreateMonster();

            m_lastSpawn = Time.time;
        }
    }

	private void CreateMonster()
    {
		GameObject monster = Instantiate(m_monsterPrefab, this.transform);
		monster.GetComponent<Monster>().SetMoveTarget(m_moveTarget.gameObject);
    }
}
