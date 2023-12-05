using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent m_agent;

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        if (destination != null)
            m_agent.SetDestination(destination.position);
    }

    void Update()
    {
        if (m_agent.hasPath && m_agent.remainingDistance <= 0.75f)
            Destroy(gameObject, 0.25f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Registering");
        if(other.TryGetComponent<Turret>(out var turret))
        {
            turret.RegisterEnemy(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Unregistering.");
        if(other.TryGetComponent<Turret>(out var turret))
        {
            turret.UnregisterEnemy(this);
        }
    }

    public void SetDestination(Vector3 position) => m_agent.SetDestination(position);

    public float GetRemainingDistance() => m_agent.remainingDistance;
}
