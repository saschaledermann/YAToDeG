using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void PauseGame(bool pause = true)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}
