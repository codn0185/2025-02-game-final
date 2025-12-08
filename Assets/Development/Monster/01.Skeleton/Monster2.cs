using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster2 : MonoBehaviour
{
    public float baseSpeed = 1;
    public float currentSpeed = 1;
    public int hp;
    Animator animator;
    bool isDead = false;
    public Slider healthBar;
    public int max_hp = 3;

    void Start()
    {
        animator = GetComponent<Animator>();
        var rd = GameManager.instance.CurrentRoundData;
        max_hp = Mathf.RoundToInt(rd.mob_hp_rate * max_hp);
        hp = max_hp;
        currentSpeed = baseSpeed * rd.mob_speed_rate;
        healthBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }
        transform.position += currentSpeed * Time.deltaTime * transform.forward;
    }

    public void OnHit(Collider other)
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

            Bullet bullet = other.GetComponentInParent<Bullet>();
            Instantiate(bullet.hitParticle, transform.position, Quaternion.identity);


            hp = Math.Max(hp - bullet.damage, 0);
            healthBar.maxValue = max_hp;
            healthBar.value = hp;
            if (hp <= 0)
            {
                SetDeath();
            }

            if (--bullet.hit_count <= 0)
            {
                bullet.OnHit();
            }
            if (bullet.isKnockback)
            {
                transform.position += bullet.knockbackPower * -transform.forward;
            }
            if (bullet.isSlow)
            {
                currentSpeed = baseSpeed * bullet.slowPower;
            }
            if (bullet.isChain)
            {

            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        OnHit(other);
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
