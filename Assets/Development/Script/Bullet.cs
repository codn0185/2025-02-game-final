using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10;
    public float life_time = 10;
    public int damage;
    //Explosion
    public bool isExplosive = false;
    public float explosionSize = 3f;
    public Vector3 explosionVector;
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
        GameObject hitParticleInstance = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(hitParticleInstance, hitParticleLinger);

        if (isExplosive)
            hitCollider.transform.localScale = explosionVector;

        Destroy(gameObject, lingerDuration);

    }

    public void Initialize(Weapon weapon)
    {
        damage = weapon.damage;
        speed = weapon.projectileSpeed;
        life_time = weapon.life_time;
        isKnockback = weapon.isKnockback;
        knockbackPower = weapon.knockbackPower;
        isSlow = weapon.isSlow;
        slowPower = weapon.slowPower;
        slowDuration = weapon.slowDuration;
        isExplosive = weapon.isExplosive;
        explosionSize = weapon.explosionSize;
        lingerDuration = weapon.lingerDuration;
        isChain = weapon.isChain;
        chainSize = weapon.chainSize;
        chainCount = weapon.chainCount;
    }


}
