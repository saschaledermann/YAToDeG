using UnityEngine;

[CreateAssetMenu(fileName = "New Turret details", menuName = "Turrets/Turret specification")]
public class TurretDetails : ScriptableObject
{
    [SerializeField] string m_name = "";
    [SerializeField] int m_damage = 0;
    [SerializeField] int m_damageIncrement = 0;
    [SerializeField][Range(0f, 20f)] float m_range = 0f;
    [SerializeField] float m_rateOfFire = 0;
    [SerializeField] float m_rateOfFireIncrement = 0;
    [SerializeField] int m_maxLevel = 0;
    [SerializeField] int m_upgradeCost = 0;
    [SerializeField] int m_costIncrement = 0;

    public string Name { get => m_name; }
    public int Damage { get => m_damage; }
    public int DamageIncrement { get => m_damageIncrement; }
    public float Range { get => m_range; }
    public float RateOfFire { get => m_rateOfFire; }
    public float RateOfFireIncrement { get => m_rateOfFireIncrement; }
    public int MaxLevel { get => m_maxLevel; }
    public int UpgradeCost { get => m_upgradeCost; }
    public int CostIncrement { get => m_costIncrement; }
}
