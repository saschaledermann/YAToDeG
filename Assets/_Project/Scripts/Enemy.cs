using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform m_destination;
    [SerializeField] int m_damage = 5;
    [SerializeField] int m_maxHealth = 100;
    int m_currentHealth;
    NavMeshAgent m_agent;

    public int Damage { get => m_damage; }

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        if (m_destination != null)
            m_agent.SetDestination(m_destination.position);
        
        m_currentHealth = m_maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Turret>(out var turret))
            turret.RegisterEnemy(this);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Turret>(out var turret))
            turret.UnregisterEnemy(this);
    }

    public void SetDestination(Vector3 position) => m_agent.SetDestination(position);

    public float GetRemainingDistance() => m_agent.remainingDistance;

    public void TakeDamage(int amount)
    {
        if (amount < 0)
            amount *= -1;
        
        m_currentHealth = m_currentHealth - amount < 0 ? 0 : m_currentHealth - amount;

        if (m_currentHealth > 0)
            Debug.Log($"{transform.name} health is now at {m_currentHealth}.");
        else
            Destroy(gameObject);
    }
}
