using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Button m_upgradeButton;
    [SerializeField] Button m_sellButton;
    [SerializeField] Button m_closeButton;
    Turret m_turretRef;
    bool m_isMoving = false;
    bool m_hidden = true;
    Vector3 m_hidePos;
    Vector3 m_showPos;
    public static UpgradeUI Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        m_hidePos = m_showPos = transform.GetChild(0).transform.localPosition;
        m_showPos.x = m_hidePos.x * -1;

        m_closeButton.onClick.AddListener(() => TogglePanel(false));
    }

    public void OpenTurretUI(Turret turret)
    {
        if (turret == null) return;

        Debug.Log("UI opened!");
        m_turretRef = turret;
        TogglePanel();
    }

    void TogglePanel(bool open = true)
    {
        if (m_isMoving || (!m_hidden && open)) return;
        StartCoroutine(MoveUI(transform.GetChild(0).GetComponent<RectTransform>()));
    }

    IEnumerator MoveUI(RectTransform rectTransform, float moveTime = 0.25f)
    {
        m_isMoving = true;
        var startPos = m_hidden ? m_hidePos : m_showPos;
        var goalPos = m_hidden ? m_showPos : m_hidePos;
        var elapsedTime = 0f;

        m_upgradeButton.interactable = false;
        m_sellButton.interactable = false;
        m_closeButton.interactable = false;

        while (elapsedTime < moveTime)
        {
            var t = 1 / moveTime * elapsedTime;
            rectTransform.localPosition = Vector2.Lerp(startPos, goalPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_upgradeButton.interactable = true;
        m_sellButton.interactable = true;
        m_closeButton.interactable = true;

        m_hidden = !m_hidden;
        m_isMoving = false;
    }
}
