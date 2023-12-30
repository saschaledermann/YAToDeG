using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPaused { get; private set; }
    public bool LevelStarted { get; private set; }
    public event Action LevelStartedEvent;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        IsPaused = true;
        LevelStarted = false;
        Time.timeScale = 1f;
    }

    public void PauseGame(bool pause = true)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
        if (!pause && !LevelStarted)
        {
            LevelStartedEvent?.Invoke();
            LevelStarted = true;
        }
    }
    public void EndLevel()
    {
        LevelStarted = false;
    }
}
