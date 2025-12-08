using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolem : BossMonsterBase
{
    // === Rock Throw & Summon Settings ===
    private const string rockThrowAnimationTrigger = "RockThrow"; // 애니메이션 트리거 이름
    [Header("Rock Golem - Rock Throw & Summon")]
    [SerializeField] private float rockThrowInterval = 5f; // 바위 투척 주기
    [SerializeField] private float rockThrowDuration = 1f; // 바위 투척 동작 시간

    [Header("Prefabs & References")]
    [SerializeField] private GameObject rockPrefab; // 던질 돌 프리퍼
    [SerializeField] private GameObject miniGolemPrefab; // 소환할 미니 골렘 프리퍼
    [SerializeField] private Transform handTransform; // 돌을 들고 던지는 손 위치

    [Header("Throw Parameters")]
    // [SerializeField] private float rockHoldDuration = 0.5f; // 손에 돌을 들고 있는 시간
    [SerializeField] private float throwDistance = 10f; // 투척 거리
    [SerializeField] private float throwHeight = 5f; // 포물선 최대 높이
    [SerializeField] private float throwSpeed = 10f; // 투척 속도
    [SerializeField] private float throwAngleRange = 30f; // 투척 각도 범위 (예: 30도 = 좌우 +-15도)

    private Coroutine rockThrowRoutine;
    private bool isThrowingRock = false;

    protected override void Awake()
    {
        base.Awake();

        // maxHealth = 1200;
        // currentHealth = maxHealth;
        // moveSpeed = 0.3f;
        // attackDamage = 25;
        // attackSpeed = 10f;
        // experiencePoints = 200;
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.ChangeState(StateMachine.MoveState);
    }

    protected override void Update()
    {
        base.Update();
    }

    // === Move State Overrides ===
    public override void OnEnterMove()
    {
        base.OnEnterMove();

        // 이동 상태 진입 시 바위 투척 루틴 시작
        StartRockThrowRoutine();
    }

    public override void OnUpdateMove()
    {
        // 바위 투척 중에는 이동하지 않음
        if (isThrowingRock)
            return;

        base.OnUpdateMove();
    }

    public override void OnExitMove()
    {
        base.OnExitMove();

        // 이동 상태 퇴출 시 바위 투척 루틴 중지
        StopRockThrowRoutine();
    }

    // === Rock Throw & Summon System ===
    private void StartRockThrowRoutine()
    {
        if (rockThrowRoutine == null)
        {
            rockThrowRoutine = StartCoroutine(RockThrowRoutine());
        }
    }

    private void StopRockThrowRoutine()
    {
        if (rockThrowRoutine != null)
        {
            StopCoroutine(rockThrowRoutine);
            rockThrowRoutine = null;
        }
        isThrowingRock = false;
    }

    private IEnumerator RockThrowRoutine()
    {
        while (true)
        {
            // 바위 투척 주기까지 대기
            yield return new WaitForSeconds(rockThrowInterval);

            // 바위 투척 시작
            isThrowingRock = true;

            // 바위 투척 애니메이션 재생
            if (Animator != null)
            {
                Animator.SetTrigger(rockThrowAnimationTrigger);
            }

            // 바위 투척 및 소형 돌골렘 소환 코루틴 시작
            StartCoroutine(ThrowRockAndSummon());

            // 바위 투척 동작 시간 대기
            yield return new WaitForSeconds(rockThrowDuration);

            // 바위 투척 종료, 이동 재개
            isThrowingRock = false;
        }
    }

    /// <summary>
    /// 돌을 투척하고 착지 지점에 미니 골렘을 소환하는 코루틴
    /// </summary>
    private IEnumerator ThrowRockAndSummon()
    {
        // 손 위치 확인
        Vector3 spawnPosition = handTransform != null ? handTransform.position : transform.position + Vector3.up * 2f;

        // 돌 프리퍼 생성
        if (rockPrefab != null)
        {
            GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            // 돌을 손에 붙여서 유지 (애니메이션이 끝날 때까지 = rockThrowDuration 동안)
            float elapsedTime = 0f;
            while (elapsedTime < rockThrowDuration)
            {
                if (rock != null && handTransform != null)
                {
                    rock.transform.position = handTransform.position;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 애니메이션 종료 시점의 손 위치에서 투척 시작
            Vector3 throwStartPosition = handTransform != null ? handTransform.position : rock.transform.position;

            // 랜덤한 각도 계산 (좌우 -throwAngleRange/2 ~ +throwAngleRange/2)
            float randomAngle = Random.Range(-throwAngleRange / 2f, throwAngleRange / 2f);

            // 골렘의 정면 방향에 랜덤 각도를 적용
            Vector3 throwDirection = Quaternion.Euler(0, randomAngle, 0) * transform.forward;

            // 목표 지점 계산 (랜덤 방향으로 throwDistance 만큼)
            Vector3 targetPosition = transform.position + throwDirection * throwDistance;
            targetPosition.y = 0; // 바닥 높이로 설정

            // 돌 투척 코루틴 시작 (손의 현재 위치에서 시작)
            yield return StartCoroutine(ProjectileMotion(rock, throwStartPosition, targetPosition));

            // 착지 지점에 미니 골렘 소환 (RockGolem과 동일한 방향)
            if (miniGolemPrefab != null)
            {
                Instantiate(miniGolemPrefab, targetPosition, transform.rotation);
            }

            // 돌 오브젝트 파괴
            if (rock != null)
            {
                Destroy(rock);
            }
        }
    }

    /// <summary>
    /// 포물선 운동을 처리하는 코루틴
    /// </summary>
    private IEnumerator ProjectileMotion(GameObject projectile, Vector3 startPosition, Vector3 targetPosition)
    {
        if (projectile == null) yield break;

        float distance = Vector3.Distance(new Vector3(startPosition.x, 0, startPosition.z),
                                         new Vector3(targetPosition.x, 0, targetPosition.z));
        float duration = distance / throwSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (projectile == null) yield break;

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // 수평 이동 (선형 보간)
            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, progress);

            // 수직 이동 (포물선 곡선)
            float height = throwHeight * Mathf.Sin(progress * Mathf.PI);
            currentPos.y += height;

            projectile.transform.position = currentPos;

            // 바닥에 도달하거나 가까워지면 조기 종료
            if (currentPos.y <= 0.1f)
            {
                projectile.transform.position = new Vector3(currentPos.x, 0, currentPos.z);
                break;
            }

            yield return null;
        }

        // 최종 위치 보정
        if (projectile != null)
        {
            projectile.transform.position = targetPosition;
        }
    }
}
