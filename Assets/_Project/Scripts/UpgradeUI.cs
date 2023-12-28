using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
#region UIElements
    [SerializeField] Button m_upgradeButton;
    [SerializeField] Button m_sellButton;
    [SerializeField] Button m_closeButton;
    [SerializeField] Image m_turretSprite;
    [SerializeField] TMP_Text m_nameText;
    [SerializeField] TMP_Text m_damageText;
    [SerializeField] TMP_Text m_rofText;
    [SerializeField] TMP_Text m_levelText;
    [SerializeField] TMP_Text m_upgradeCostText;
    [SerializeField] TMP_Text m_sellText;
#endregion
    
    Turret m_turretRef;
    bool m_isMoving = false;
    bool m_hidden = true;
    Vector3 m_hidePos;
    Vector3 m_showPos;
    public static UpgradeUI Instance { get; private set; }

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

        m_upgradeButton.onClick.AddListener(() => UpgradeTurret());
        m_closeButton.onClick.AddListener(() => TogglePanel(false));
    }

    public void OpenTurretUI(Turret turret)
    {
        if (turret == null) return;

        m_turretRef = turret;
        UpdateTurretUiInfo(turret);
        TogglePanel();
    }

    void UpgradeTurret()
    {
        if (m_turretRef == null) return;

        m_turretRef.Upgrade();
        UpdateTurretUiInfo(m_turretRef);
    }

    void UpdateTurretUiInfo(Turret turret)
    {
        if (turret == null) return;

        m_nameText.text = $"Turret: {turret.Name}";
        m_damageText.text = $"Damage: {turret.Damage}";
        m_rofText.text = $"Rate of fire: {turret.RateOfFire}";
        m_levelText.text = $"Level: {turret.Level} / {turret.MaxLevel}";
        m_upgradeCostText.text = $"Upgrade cost: {turret.UpgradeCost}";
        m_sellText.text = $"Sells for: {turret.SellAmount}";
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
