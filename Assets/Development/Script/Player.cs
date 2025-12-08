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
    private Weapon weapon;
    // Spell effects
    public GameObject[] bulletPrefabs;
    private static WeaponBaseSettingSO weaponBaseSettings;

    // static float bullet_gap = 0.25f;
    // Particle
    public ParticleSystem LevelUp_Particle;
    // Audio
    public SoundManager soundManager;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (weaponBaseSettings == null)
        {
            weaponBaseSettings = Resources.Load<WeaponBaseSettingSO>("WeaponBase");
        }

        weapon = weaponBaseSettings.weapons[attack_type];
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

                AnimatorChange("RUN");
            }
            else if (distance > 0.1f)
            {

                Vector3 velocity = rb.velocity;
                velocity.x = -speed;
                rb.velocity = velocity;

                AnimatorChange("RUN");
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

        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);

        animator.SetBool(temp, true);
    }

    void Attack()
    {
        AnimatorChange("SHOOT");
        // foreach (float posX in BulletPosX())
        // {
        GameObject bullet = Instantiate(bulletPrefabs[attack_type], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 1.0f), Quaternion.identity);

        bullet.GetComponent<Bullet>().Initialize(weapon);
        // }
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