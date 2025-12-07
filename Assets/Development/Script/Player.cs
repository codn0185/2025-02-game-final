using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    public float moveDistance;
    // Player
    public float speed;
    // Spell effects
    public GameObject[] bulletPrefabs;
    static float bullet_gap = 0.25f;
    // Particle
    public ParticleSystem LevelUp_Particle;
    // Audio
    public SoundManager soundManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

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
        SoundManager.instance.AudioStart(SoundManager.AudioValue.Shoot);
        foreach (float posX in BulletPosX())
        {
            GameObject bullet = Instantiate(bulletPrefabs[GameManager.instance.attack_type], new Vector3(transform.position.x + posX, transform.position.y + 0.5f, transform.position.z + 1.0f), Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(
                damage: GameManager.instance.bullet_damage,
                hit_count: GameManager.instance.bullet_hit_count
            );
        }
    }

    IEnumerator Attack_Coroutine()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY)
        {
            Attack();
            yield return new WaitForSeconds(5.0f / GameManager.instance.attack_speed);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Attack_Coroutine());
    }

    float[] BulletPosX()
    {
        int bullet_count = GameManager.instance.bullet_count;
        float[] posX = new float[bullet_count];
        float x = -bullet_gap * (bullet_count / 2);
        for (int i = 0; i < bullet_count; i++)
        {
            posX[i] = x;
            x += bullet_gap;
        }

        return posX;
    }

    private void LevelUp(GameObject gameObject)
    {
        LevelUp_Particle.Play();
        SoundManager.instance.AudioStart(SoundManager.AudioValue.Get);
        Destroy(gameObject);
    }
}