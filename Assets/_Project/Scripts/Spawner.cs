using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform destination;
    public int maxNumberToSpawn = 10;

    int m_spawnCount = 0;

    void Start()
    {
        if (enemyPrefab != null)
        {
            InvokeRepeating(nameof(Spawn), 1f, 0.5f);
        }
    }

    void Spawn()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return; 
        var spawnPos = transform.position;
        spawnPos.y = 0f;
        var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetDestination(destination.position);
        m_spawnCount++;
        if (m_spawnCount >= maxNumberToSpawn)
            CancelInvoke(nameof(Spawn));
    }
}
