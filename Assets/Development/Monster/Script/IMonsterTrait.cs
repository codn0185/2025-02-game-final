using System.Collections;
using UnityEngine;

public interface IMonsterTrait
{
    // 몬스터 특성 인터페이스
}

/// <summary>
/// 몬스터가 정지(멈춤) 가능함을 나타내는 인터페이스
/// </summary>
public interface IStoppable : IMonsterTrait
{
    /// <param name="duration"> 정지 지속 시간 (초)</param>
    void ApplyStop(float duration); // 정지 동작 구현
    IEnumerator StopCoroutine(float duration); // 정지 코루틴 구현
}

/// <summary>
/// 몬스터가 넉백(밀려남) 가능함을 나타내는 인터페이스
/// </summary>
public interface IKnockbackable : IMonsterTrait
{
    /// <param name="direction"> 넉백 방향</param>
    /// <param name="force"> 넉백 세기</param>
    void ApplyKnockback(Vector3 direction, float force); // 넉백 동작 구현
}

/// <summary>
/// 몬스터가 느려짐(슬로우) 가능함을 나타내는 인터페이스
/// </summary>
public interface ISlowable : IMonsterTrait
{
    /// <param name="slowPercentage"> 느려짐 비율 (0~1)</param>
    /// <param name="duration"> 느려짐 지속 시간 (초)</param>
    void ApplySlow(float slowPercentage, float duration); // 느려짐 동작 구현
    IEnumerator SlowCoroutine(float slowPercentage, float duration); // 느려짐 코루틴 구현
}
