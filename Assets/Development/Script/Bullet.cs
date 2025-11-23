using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float life_time ;
    public int damage ;
    public int penetration_count ;

    void Start()
    {
        Destroy(gameObject, life_time);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    public void Initialize(int damage = 1, int penetration_count = 1, float speed = 15, float life_time = 10f)
    {
        this.damage = damage;
        this.penetration_count = penetration_count;
        this.speed = speed;
        this.life_time = life_time;
    }
}
