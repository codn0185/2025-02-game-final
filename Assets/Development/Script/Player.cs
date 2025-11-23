using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigidbody;
    Animator animator;

    Vector3 startPosition;
    Vector3 endPosition;

    // Player
    public float speed;

    // Bullet
    public GameObject bulletPrefab;
    static float bullet_gap = 0.25f;

    // Particle
    public ParticleSystem LevelUp_Particle;

    // Audio
    public SoundManager audio;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        StartCoroutine(Bullet_Coroutine());
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState != GameState.GAME_PLAY)
        {
            return;
        }
        // 마우스 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            if (Vector3.Distance(startPosition, endPosition) > 0.5f)
            {
                Vector3 distance = endPosition - startPosition;
                int sign = (int)Mathf.Sign(distance.x);
                if (sign == 1) // 오른쪽 스와이프
                {
                    startPosition.x = endPosition.x - 10.0f;

                    Vector3 v = rigidbody.velocity;
                    v.x = speed;
                    rigidbody.velocity = v;

                    AnimatorChange("RUN");
                }
                else if (sign == -1) // 왼쪽 스와이프
                {
                    startPosition.x = endPosition.x + 10.0f;

                    Vector3 v = rigidbody.velocity;
                    v.x = -speed;
                    rigidbody.velocity = v;

                    AnimatorChange("RUN");
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
            rigidbody.velocity = Vector3.zero;

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

    void Bullet_Make()
    {
        AnimatorChange("SHOOT");
        SoundManager.instance.AudioStart(SoundManager.AudioValue.Shoot);
        foreach (float posX in BulletPosX())
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x + posX, transform.position.y + 0.5f, transform.position.z + 1.0f), Quaternion.identity);
            bullet.GetComponent<Bullet>().Initialize(
                damage: GameManager.instance.bullet_damage,
                penetration_count: GameManager.instance.bullet_penetration_count
            );
        }
    }

    IEnumerator Bullet_Coroutine()
    {
        if (GameManager.instance != null && GameManager.instance.CurrentState == GameState.GAME_PLAY)
        {
            Bullet_Make();
            yield return new WaitForSeconds(GameManager.instance.bullet_time);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(Bullet_Coroutine());
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