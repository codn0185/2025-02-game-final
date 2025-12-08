using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    public float moveDistance = 10;
    // Player
    public float speed;
    public float attack_speed;
    public int attack_type;
    private WeaponStats weapon;
    // Spell effects
    public GameObject[] bulletPrefabs;
    private static WeaponBaseSettingSO weaponBaseSettings;

    // static float bullet_gap = 0.25f;
    // Particle
    public ParticleSystem LevelUp_Particle;
    // Audio
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (weaponBaseSettings == null)
        {
            weaponBaseSettings = Resources.Load<WeaponBaseSettingSO>("WeaponBase");
        }

        weapon = weaponBaseSettings.weapons[attack_type];

        // 공격 속도 업그레이드 적용
        attack_speed = weapon.attack_speed;


        StartCoroutine(Attack_Coroutine());
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState != GameState.GAME_PLAY)
        {
            return;
        }
        // 마우스 좌클릭
        // Moves to clicked position
        if (Input.GetMouseButton(0))
        {
            float destX = (Input.mousePosition.x / Screen.width * moveDistance) - moveDistance / 2;

            float distance = rb.position.x - destX;

            // If the distance isn't that big
            if (distance < -0.1f)
            {
                Vector3 velocity = rb.velocity;
                velocity.x = speed;
                rb.velocity = velocity;

                animator.SetBool("walk", true);
                animator.SetBool("right", true);
                animator.SetBool("left", false);
            }
            else if (distance > 0.1f)
            {

                Vector3 velocity = rb.velocity;
                velocity.x = -speed;
                rb.velocity = velocity;


                animator.SetBool("walk", true);
                animator.SetBool("right", true);
                animator.SetBool("left", false);
            }
            else
            {
                rb.velocity = Vector3.zero;

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = Vector3.zero;

            AnimatorChange("IDLE");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.GetItem(other.tag))
        {
            LevelUp(other.gameObject);
        }
    }

    void AnimatorChange(string temp)
    {
        if (temp == "SHOOT")
        {
            animator.SetTrigger(temp);
            return;
        }

        animator.SetBool("walk", false);

        animator.SetBool(temp, true);
    }

    void Attack()
    {
        AnimatorChange("attack");

        GameObject bullet = Instantiate(bulletPrefabs[attack_type], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 1.0f), Quaternion.identity);

        // 업그레이드가 적용된 무기 데이터 생성
        WeaponStats upgradedWeaponStats = ApplyUpgradesToWeaponStats(weapon);
        bullet.GetComponent<Bullet>().Initialize(upgradedWeaponStats);
    }

    /// <summary>
    /// 공격 속도 업그레이드 적용
    /// </summary>
    public void ApplyAttackSpeedUpgrade()
    {
        switch (weapon.magicType)
        {
            case MagicType.Earth:
                attack_speed *= MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Earth_AttackSpeed);
                break;
            case MagicType.Ice:
                attack_speed *= MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Ice_AttackSpeed);
                break;
                // Fire와 Lightning은 공속 업그레이드 없음
        }
    }

    /// <summary>
    /// 무기 데이터에 업그레이드 적용
    /// </summary>
    private WeaponStats ApplyUpgradesToWeaponStats(WeaponStats baseWeaponStats)
    {
        // 원본 데이터를 복사하여 수정 (원본 SO 보존)
        WeaponStats upgraded = Instantiate(baseWeaponStats);

        switch (weapon.magicType)
        {
            case MagicType.Earth:
                // 넉백 파워 증가
                upgraded.knockbackPower *= MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Earth_Knockback);
                break;

            case MagicType.Ice:
                // 감속 효과 증가
                upgraded.slowPower = Mathf.RoundToInt(upgraded.slowPower * MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Ice_SlowIntensity));
                break;

            case MagicType.Fire:
                // 공격력 증가
                upgraded.damage = Mathf.RoundToInt(upgraded.damage * MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Fire_AttackPower));
                // 폭발 범위 증가
                upgraded.explosionSize *= MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Fire_ExplosionRadius);
                break;

            case MagicType.Lighting:
                // 공격력 증가
                upgraded.damage = Mathf.RoundToInt(upgraded.damage * MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Lightning_AttackPower));
                // 연쇄 개수 증가
                upgraded.chainCount = Mathf.RoundToInt(upgraded.chainCount * MetaManager.Instance.GetUpgradeMultiplier(MetaUpgradeType.Lightning_ChainCount));
                break;
        }

        return upgraded;
    }

    IEnumerator Attack_Coroutine()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY)
        {
            Attack();
            yield return new WaitForSeconds(5.0f / attack_speed);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Attack_Coroutine());
    }

    // float[] BulletPosX()
    // {
    //     int bullet_count = 1;
    //     float[] posX = new float[bullet_count];
    //     float x = -bullet_gap * (bullet_count / 2);
    //     for (int i = 0; i < bullet_count; i++)
    //     {
    //         posX[i] = x;
    //         x += bullet_gap;
    //     }

    //     return posX;
    // }

    private void LevelUp(GameObject gameObject)
    {
        LevelUp_Particle.Play();
        SoundManager.instance.AudioStart(SoundManager.AudioValue.Get);
        Destroy(gameObject);
    }

    public void SetAttackType(int type)
    {
        attack_type = type;
    }
}