using System;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float speed;
    public int hp;
    Animator animator;
    bool isDead = false;
    public Slider healthBar;
    public int max_hp = 3;
    public GameObject hitParticle;

    void Start()
    {
        animator = GetComponent<Animator>();

        var rd = GameManager.instance.CurrentRoundData;
        max_hp = Mathf.RoundToInt(rd.mob_hp_rate * max_hp);
        hp = max_hp;
        speed = speed * rd.mob_speed_rate;
        healthBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (isDead)
            {
                return;
            }
            if (!healthBar.gameObject.activeSelf)
            {
                healthBar.gameObject.SetActive(true);
            }

            Instantiate(hitParticle, other.gameObject.transform.position, Quaternion.identity);
            SoundManager.instance.AudioStart(SoundManager.AudioValue.Hit);

            Bullet bullet = other.GetComponentInParent<Bullet>();

            hp = Math.Max(hp - bullet.damage, 0);
            healthBar.maxValue = max_hp;
            healthBar.value = hp;
            if (hp <= 0)
            {
                SetDeath();
            }

            if (--bullet.penetration_count <= 0)
            {
                Destroy(other.transform.parent.gameObject);
            }
        }
    }

    void SetDeath()
    {
        isDead = true;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
        healthBar.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        Destroy(gameObject, 10f);
        GameManager.instance.IncreaseKillCount();
    }
}
