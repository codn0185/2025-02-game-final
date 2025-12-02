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
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }
}