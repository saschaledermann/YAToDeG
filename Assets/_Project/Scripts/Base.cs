using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] int m_maxHealth = 100;
    int m_currentHealth;

    void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        var enemyDamage = other.GetComponent<Enemy>().Damage;
        m_currentHealth = m_currentHealth - enemyDamage < 0 ? 0 : m_currentHealth - enemyDamage;
        if (m_currentHealth == 0)
            Debug.LogError($"GAME OVER!");
        else
            Debug.LogWarning($"Health is at {m_currentHealth}");

        Destroy(other.gameObject);
    }
}
