using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CommonMonsterBase : MonsterController, IStoppable, IKnockbackable, ISlowable
{

    private IEnumerator slowCoroutineTracker;
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

    public void ApplySlow(float slowPower, float duration)
    {
        if (moveSpeed < baseMoveSpeed)
            StopCoroutine(slowCoroutineTracker);
        slowCoroutineTracker = SlowCoroutine(slowPower, duration);
        StartCoroutine(slowCoroutineTracker);
    }

    public IEnumerator SlowCoroutine(float slowPower, float duration)
    {
        moveSpeed = baseMoveSpeed * (0.3f + (1 / (2 + slowPower)));
        yield return new WaitForSeconds(duration);
        moveSpeed = baseMoveSpeed;
    }



    new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag(Tag.Bullet))
            OnHit(other);
    }

    new void OnHit(Collider other)
    {
        Bullet bullet = other.GetComponentInParent<Bullet>();

        if (bullet.isKnockback)
        {
            transform.position += bullet.knockbackPower * -transform.forward;
        }
        if (bullet.isSlow)
        {
            ApplySlow(bullet.slowPower, bullet.slowDuration);
        }

    }

}