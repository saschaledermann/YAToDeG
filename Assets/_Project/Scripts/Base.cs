using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] ControlUI m_controlUI;
    [SerializeField] int m_maxHealth = 100;
    int m_currentHealth;

    void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    void Start()
    {
        m_controlUI.SetHealthText(m_currentHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        var enemyDamage = other.GetComponent<Enemy>().Damage;
        m_currentHealth = m_currentHealth - enemyDamage < 0 ? 0 : m_currentHealth - enemyDamage;
        if (m_currentHealth == 0)
            m_controlUI.SetHealthText(0);
        else
            m_controlUI.SetHealthText(m_currentHealth);

        Destroy(other.gameObject);
    }
}
