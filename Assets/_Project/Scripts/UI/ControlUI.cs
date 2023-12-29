using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    [SerializeField] Button m_pauseButton;
    [SerializeField] Button m_menuButton;

    void Start()
    {
        m_pauseButton.onClick.AddListener(() => ToggleGame());
        m_menuButton.onClick.AddListener(() => BackToMenu());
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
}
