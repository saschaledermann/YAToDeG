using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform m_spawnPosition;
    [SerializeField] Transform m_goalPosition;
    [SerializeField][Range(0.25f, 2f)] float m_spawnInterval;
    [SerializeField][Range(1f, 5f)] float m_waveInterval;
    [SerializeField] Level[] m_levels;
    [SerializeField] ControlUI m_controlUI;

    int m_enemyCount = 0;
    int m_currentLevel = 0;

    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LevelStartedEvent += StartLevel;
        m_controlUI.SetLevelText(m_currentLevel, m_levels.Length);
    }

    void StartLevel()
    {
        m_currentLevel++;
        if (m_currentLevel > m_levels.Length) return;
        m_controlUI.SetLevelText(m_currentLevel, m_levels.Length);
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        var enemyTotal = 0;
        foreach (var wave in m_levels[m_currentLevel-1].waves)
        {
            enemyTotal += wave.spawnAmount;
        }
        m_enemyCount = enemyTotal;

        foreach (var wave in m_levels[m_currentLevel-1].waves)
        {
            for (int i = 0; i < wave.spawnAmount; i++)
            {
                SpawnEnemy(wave.enemyPrefab);
                yield return new WaitForSeconds(m_spawnInterval);
            }
            yield return new WaitForSeconds(m_waveInterval);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        var enemy = Instantiate(enemyPrefab, m_spawnPosition.position, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetDestination(m_goalPosition.position);
    }
    
    void DecrementEnemyCount()
    {
        m_enemyCount--;

        if (m_enemyCount <= 0 && GameManager.Instance != null)
            GameManager.Instance.EndLevel();
    }

    void OnEnable()
    {
        Enemy.EnemyDieEvent += DecrementEnemyCount;
    }

    void OnDisable()
    {
        Enemy.EnemyDieEvent -= DecrementEnemyCount;
        if (GameManager.Instance != null)
            GameManager.Instance.LevelStartedEvent -= StartLevel;
    }
}
