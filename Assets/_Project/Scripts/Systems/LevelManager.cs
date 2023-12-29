using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Level[] m_levels;
    
    void DecrementEnemyCount()
    {
        Debug.Log("Enemy died!");
    }

    void OnEnable()
    {
        Enemy.EnemyDieEvent += DecrementEnemyCount;
    }

    void OnDisable()
    {
        Enemy.EnemyDieEvent -= DecrementEnemyCount;
    }
}
