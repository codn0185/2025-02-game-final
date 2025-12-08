using UnityEngine;

/// <summary>
/// 화염 투사체 - 앞으로 날아가며 플레이어와 충돌 시 데미지 적용
/// </summary>
public class FlameProjectile : MonoBehaviour
{
    private int damage;
    private float speed;

    // FireDragon에서 호출하여 투사체 초기화
    public void Initialize(int damage, float speed, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        // Rigidbody로 앞으로 발사
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // 수명 후 자동 제거
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 감지
        if (other.CompareTag(Tag.Player))
        {
            // 데미지 적용
            ApplyDamage(other.gameObject);

            // 투사체 제거
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 플레이어에게 데미지 적용
    /// </summary>
    private void ApplyDamage(GameObject player)
    {
        // TODO: 플레이어 체력 시스템 연동
        // PlayerController playerController = player.GetComponent<PlayerController>();
        // if (playerController != null)
        // {
        //     playerController.TakeDamage(damage);
        // }

        Debug.Log($"Flame projectile hit player! Damage: {damage}");
    }
}
