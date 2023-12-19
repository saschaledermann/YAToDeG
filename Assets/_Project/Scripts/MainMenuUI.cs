using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] RectTransform m_buttonContainer;
    [SerializeField] RectTransform m_highscoreContainer;
    [SerializeField] TMP_InputField m_nameInputfield;
    [SerializeField] Button m_startButton;
    [SerializeField] Button m_highscoreButton;
    [SerializeField] Button m_quitButton;
    [SerializeField] Button m_backButton;
    [SerializeField] TMP_Text m_firstHighscoreText;
    [SerializeField] TMP_Text m_secondHighscoreText;
    [SerializeField] TMP_Text m_thirdHighscoreText;
    Vector2 m_showPosition;
    Vector2 m_hidePosition;

    void Start()
    {
        m_highscoreContainer.localPosition = new Vector2(-1000f, m_highscoreContainer.localPosition.y);
        m_showPosition = m_buttonContainer.localPosition;
        m_hidePosition = m_highscoreContainer.localPosition;
        m_backButton.interactable = false;
        m_startButton.onClick.AddListener(() => 
        {
            SaveName();
            StartGame();
        });
        m_highscoreButton.onClick.AddListener(() => ShowHighscores());
        m_quitButton.onClick.AddListener(() => QuitGame());
        m_backButton.onClick.AddListener(() => ShowButtonContainer());
        m_nameInputfield.text = PlayerPrefs.GetString("currentPlayer");
    }

    void ShowButtonContainer()
    {
        ToggleButtonInteractablility();
        StartCoroutine(moveContainer(m_highscoreContainer, false));
        StartCoroutine(moveContainer(m_buttonContainer, true));
    }

    void ShowHighscores()
    {
        ToggleButtonInteractablility();
        StartCoroutine(moveContainer(m_buttonContainer, false));
        StartCoroutine(moveContainer(m_highscoreContainer, true));
    }

    void ToggleButtonInteractablility()
    {
        m_startButton.interactable = !m_startButton.interactable;
        m_highscoreButton.interactable = !m_highscoreButton.interactable;
        m_quitButton.interactable = !m_quitButton.interactable;
        m_backButton.interactable = !m_backButton.interactable;
    }

    IEnumerator moveContainer(RectTransform rectTransform, bool show = true, float moveTime = 0.25f)
    {
        var elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            var t = 1 / moveTime * elapsedTime;
            rectTransform.localPosition = Vector2.Lerp(show ? m_hidePosition : m_showPosition, show ? m_showPosition : m_hidePosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void SaveName()
    {
        if (string.IsNullOrWhiteSpace(m_nameInputfield.text)) return;

        PlayerPrefs.SetString("currentPlayer", m_nameInputfield.text);
    }

    void StartGame()
    {
        // if (SceneManager.sceneCount <= 1) return;

        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
