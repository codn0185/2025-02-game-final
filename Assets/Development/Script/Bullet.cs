using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public float life_time = 10;
    public int damage;
    public int hit_count;
    //Explosion
    public bool isExplosive = false;
    public float explosionSize = 3f;
    private Vector3 explosionVector;

    public float lingerDuration = 0f;
    // Knockback
    public bool isKnockback = false;
    public float knockbackPower = 1;
    // Slow
    public bool isSlow = false;
    public int slowPower = 1;
    public float slowDuration = 1.0f;
    public bool isChain = false;
    public float chainSize = 3;
    public int chainCount = 3;
    public GameObject hitParticle;
    public float hitParticleLinger = 0.5f;
    public CapsuleCollider hitCollider;
    public GameObject slowParticle;
    public AudioClip shootSFX;
    public AudioClip hitSFX;

    void Awake()
    {
        SoundManager.instance.PlayAudio(shootSFX);
        if (isExplosive)
            explosionVector = new Vector3(explosionSize, explosionSize, explosionSize);
        Destroy(gameObject, life_time);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    public void OnHit()
    {
        SoundManager.instance.PlayAudio(hitSFX);
        hit_count--;
        GameObject hitParticleInstance = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(hitParticleInstance, hitParticleLinger);

        if (isExplosive)
            hitCollider.transform.localScale = explosionVector;

        if (hit_count <= 0)
            Destroy(gameObject, lingerDuration);

    }

    public void Initialize(int damage = 1, int hit_count = 1, float speed = 15, float life_time = 10f)
    {
        this.damage = damage;
        this.hit_count = hit_count;
        this.speed = speed;
        this.life_time = life_time;
    }


}
