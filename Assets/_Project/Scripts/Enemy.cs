using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform m_destination;
    [SerializeField] int m_damage = 5;
    [SerializeField] int m_maxHealth = 100;
    [SerializeField] Healthbar m_healthbar;
    int m_currentHealth;
    NavMeshAgent m_agent;

    public int Damage { get => m_damage; }
    public static event Action EnemyDieEvent;

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        if (m_destination != null)
            m_agent.SetDestination(m_destination.position);
        
        m_currentHealth = m_maxHealth;
    }

    void Start()
    {
        m_healthbar.Init(m_maxHealth);
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
        m_healthbar.SetValue(m_currentHealth);

        if (m_currentHealth <= 0)
            Destroy(gameObject);
    }

    void OnDisable()
    {
        EnemyDieEvent?.Invoke();
    }
}
