using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CommonMonsterBase : MonsterController, IStoppable, IKnockbackable, ISlowable
{
    public void ApplyStop(float duration)
    {
        StartCoroutine(StopCoroutine(duration));
    }

    public IEnumerator StopCoroutine(float duration)
    {
        // 현재 상태 저장 후 Idle로 전환
        MonsterState prevState = StateMachine.CurrentState;
        StateMachine.ChangeState(StateMachine.IdleState);
        yield return new WaitForSeconds(duration);
        // 이전 상태로 복귀
        StateMachine.ChangeState(prevState);
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        transform.position += force * direction.normalized;
    }

    public void ApplySlow(float slowPercentage, float duration)
    {
        StartCoroutine(SlowCoroutine(slowPercentage, duration));
    }

    public IEnumerator SlowCoroutine(float slowPercentage, float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed = originalSpeed * (1f - Mathf.Clamp01(slowPercentage));
        Debug.Log(moveSpeed);
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    protected new void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Tag.AttackPoint))
        {
            StateMachine.ChangeState(StateMachine.BattleState);
        }
        else if (other.CompareTag(Tag.Bullet))
            OnHit(other);

    }

    public void ApplyChain(int chainCount, Bullet bullet, CommonMonsterBase avoid)
    {
        StartCoroutine(ChainCoroutine(chainCount, bullet, avoid));
    }

    IEnumerator ChainCoroutine(int chainCount, Bullet bullet, CommonMonsterBase avoid)
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 pos = transform.position;
        CommonMonsterBase closestEnemy = this;
        float closest = float.PositiveInfinity;

        foreach (CommonMonsterBase enemy in Entities)
        {
            if (!ReferenceEquals(enemy, this) && !ReferenceEquals(avoid, enemy))
            {
                var dist = (pos - enemy.transform.position).sqrMagnitude;
                if (dist < bullet.chainSize && dist < closest)
                {
                    closestEnemy = enemy;
                    closest = dist;
                }
            }
        }
        if (!ReferenceEquals(closestEnemy, this))
        {
            closestEnemy.TakeDamage(bullet.damage);
            Instantiate(bullet.hitParticle, transform.position, Quaternion.identity);

            if (chainCount > 0)
                closestEnemy.ApplyChain(chainCount - 1, bullet, this);
        }

    }


    void OnHit(Collider other)
    {
        if (other.CompareTag(Tag.Bullet))
        {
            if (IsDead)
            {
                return;
            }

            Bullet bullet = other.GetComponentInParent<Bullet>();

            TakeDamage(bullet.damage);

            if (bullet.isKnockback)
            {
                transform.position += bullet.knockbackPower * -transform.forward;
            }
            if (bullet.isSlow)
            {
                ApplySlow(bullet.slowPower, bullet.slowDuration);
            }
            if (bullet.isChain)
            {
                ApplyChain(bullet.chainCount - 1, bullet, this);
            }

            bullet.OnHit();

        }
    }
}