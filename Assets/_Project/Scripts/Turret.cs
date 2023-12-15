using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Range(3f, 15f)] public float range = 7.5f;
    [SerializeField] Transform m_tower;
    [SerializeField] Transform m_gun;
    [SerializeField] AudioClip m_shotSound;

    [SerializeField] int m_damage = 5;
    public int Damage { get => m_damage; }
    [SerializeField] float m_cooldown = 0.25f;
    float m_lastShotTime = -1f;

    List<Enemy> m_enemies = new();
    Enemy m_target;
    ParticleSystem[] m_particleSystems;


    void Start()
    {
        var sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = range;
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
        var targetPos = m_target.transform.position;
        var turretAimAt = new Vector3(targetPos.x,
                                m_tower.position.y,
                                targetPos.z);
        m_tower.LookAt(turretAimAt);

        var gunAimAt = new Vector3(targetPos.x,
                                targetPos.y,
                                targetPos.z);
        m_gun.LookAt(gunAimAt);
    }

    void ShootAtTarget()
    {
        if (!(Mathf.Abs(m_lastShotTime - Time.time) > m_cooldown)) return;

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
        m_tower.localRotation = Quaternion.identity;
        m_gun.localRotation = Quaternion.identity;
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
    
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0f, transform.position.z), range);
        }
    }
}
