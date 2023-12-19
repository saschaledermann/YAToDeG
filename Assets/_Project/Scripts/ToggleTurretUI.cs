using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTurretUI : MonoBehaviour
{
    [SerializeField] RectTransform m_turretUI;
    bool m_hidden = false;
    bool m_isMoving = false;
    Vector2 m_showPos;
    Vector2 m_hidePos;
    Button m_button;
    TMP_Text m_buttonText;

    void Start()
    {
        m_button = GetComponent<Button>();
        m_buttonText = GetComponentInChildren<TMP_Text>();
        m_showPos = m_hidePos = m_turretUI.position;
        m_hidePos.x *= -1;
    }

    public void Toggle()
    {
        if (!m_isMoving)
            StartCoroutine(MoveUI(m_turretUI));
    }

    IEnumerator MoveUI(RectTransform rectTransform, float moveTime = 0.25f)
    {
        m_isMoving = true;
        var startPos = m_hidden ? m_hidePos : m_showPos;
        var goalPos = m_hidden ? m_showPos : m_hidePos;
        var elapsedTime = 0f;

        m_button.interactable = false;

        while (elapsedTime < moveTime)
        {
            var t = 1 / moveTime * elapsedTime;
            rectTransform.position = Vector2.Lerp(startPos, goalPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_button.interactable = true;
        m_buttonText.text = m_hidden ? "<" : ">";

        m_hidden = !m_hidden;
        m_isMoving = false;
    }
}
