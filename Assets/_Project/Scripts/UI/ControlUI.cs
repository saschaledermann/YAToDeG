using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    [SerializeField] Button m_pauseButton;
    [SerializeField] Button m_menuButton;
    [SerializeField] TMP_Text m_coinAmountText;
    [SerializeField] TMP_Text m_levelText;
    [SerializeField] TMP_Text m_healthText;

    void Start()
    {
        m_pauseButton.onClick.AddListener(() => ToggleGame());
        m_menuButton.onClick.AddListener(() => BackToMenu());
        m_coinAmountText.text = "-";
        m_levelText.text = "0/5";
    }

    void ToggleGame()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame(false);
            m_pauseButton.GetComponentInChildren<TMP_Text>().text = "II";
        }
        else
        {
            GameManager.Instance.PauseGame();
            m_pauseButton.GetComponentInChildren<TMP_Text>().text = "I>";
        }
    }

    void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SetLevelText(int currentLevel, int maxLevel)
    {
        m_levelText.text = $"{currentLevel}/{maxLevel}";
    }
    
    public void SetHealthText(int currentHealth)
    {
        m_healthText.text = $"HP: {currentHealth}";
    }
}
