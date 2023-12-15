using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider m_healthbar;
    Transform m_cameraTransform;

    void Start()
    {
        m_cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - m_cameraTransform.position);
    }

    public void Init(int maxHealth)
    {
        m_healthbar.maxValue = maxHealth;
        m_healthbar.value = maxHealth;
    }

    public void SetValue(int health) => m_healthbar.value = health;
}
