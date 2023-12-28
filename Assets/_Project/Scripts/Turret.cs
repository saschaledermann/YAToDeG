using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Turret : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Transform m_tower;
    [SerializeField] Transform m_gun;
    [SerializeField] AudioClip m_shotSound;

    [SerializeField] TurretDetails m_turretDetails;
    public string Name { get => m_turretDetails.Name; }
    public int Damage { get => m_turretDetails.Damage + (m_currentLevel - 1) * m_turretDetails.DamageIncrement; }
    public float RateOfFire { get => m_turretDetails.RateOfFire + (m_currentLevel - 1) * m_turretDetails.RateOfFireIncrement; }
    public int UpgradeCost { get => m_turretDetails.UpgradeCost; }
    public int SellAmount { get => m_turretDetails.UpgradeCost - ((int) Mathf.Floor(m_turretDetails.UpgradeCost / 2)); }
    public int MaxLevel { get => m_turretDetails.MaxLevel; }
    int m_currentLevel = 1;
    public int Level { get => m_currentLevel; }
    float m_lastShotTime = -1f;
    readonly float m_rotationSpeed = 5;

    List<Enemy> m_enemies = new();
    Enemy m_target;
    ParticleSystem[] m_particleSystems;

    void Start()
    {
        var sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = m_turretDetails.Range;
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        EvaluateTarget();
        if (m_target != null)
        {
            AimAtTarget();
            ShootAtTarget();
        }
        else
        {
            Idle();
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null || m_enemies.Contains(enemy)) return;
        m_enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemy == null || !m_enemies.Contains(enemy)) return;
        m_enemies.Remove(enemy);
        if (enemy == m_target)
            m_target = null;
    }

    void AimAtTarget()
    {
        var step = m_rotationSpeed * Time.deltaTime;
        var targetPos = m_target.transform.position;
        var turretAimAt = new Vector3(targetPos.x,
                                m_tower.position.y,
                                targetPos.z);
        m_tower.rotation = Quaternion.Slerp(m_tower.rotation, Quaternion.LookRotation(turretAimAt - m_tower.position), step);

        var gunAimAt = new Vector3(targetPos.x,
                                targetPos.y,
                                targetPos.z);
        var gunAimDir = Quaternion.LookRotation(gunAimAt - m_gun.position);
        var localGunAimDir = Quaternion.Euler(new Vector3(gunAimDir.eulerAngles.x, 0, 0));
        m_gun.localRotation = Quaternion.Slerp(m_gun.localRotation, localGunAimDir, step);
    }

    void ShootAtTarget()
    {
        if (!(Mathf.Abs(m_lastShotTime - Time.time) > (1 / RateOfFire))) return;

        foreach (var ps in m_particleSystems)
        {
            ps.Play();
        }
        PlaySound();
        m_target.TakeDamage(Damage);
        m_lastShotTime = Time.time;
    }

    void Idle()
    {
        var step = Time.deltaTime;
        m_tower.localRotation = Quaternion.Slerp(m_tower.localRotation, Quaternion.identity, step);
        m_gun.localRotation = Quaternion.Slerp(m_gun.localRotation, Quaternion.identity, step);
    }

    void EvaluateTarget()
    {
        if (m_enemies.Count == 0)
            m_target = null;
        else
        {
            Enemy desiredTarget = null;
            foreach (var enemy in m_enemies)
            {
                if (desiredTarget == null)
                {
                    desiredTarget = enemy;
                    continue;
                }
                if (enemy.GetRemainingDistance() < desiredTarget.GetRemainingDistance())
                {
                    desiredTarget = enemy;
                }
            }
            m_target = desiredTarget;
        }
    }

    void PlaySound()
    {
        var soundSource = new GameObject("shot", typeof(AudioSource)).GetComponent<AudioSource>();
        soundSource.gameObject.transform.parent = transform;
        soundSource.clip = m_shotSound;
        soundSource.Play();
        Destroy(soundSource.gameObject, m_shotSound.length);
    }
    
    public void Upgrade()
    {
        if (m_currentLevel >= m_turretDetails.MaxLevel) return;

        m_currentLevel++;
    }
    
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), m_turretDetails.Range);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UpgradeUI.Instance == null) return;

        UpgradeUI.Instance.OpenTurretUI(this);
    }
}
