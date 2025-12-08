using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragon : BossMonsterBase
{
    private const string flameBreathAnimationTrigger = "FlameBreath";
    
    [Header("Fire Dragon - Flame Breath Attack")]
    [SerializeField] private float flameAttackInterval = 5f; // 화염 브레스 공격 주기
    [SerializeField] private float attackDuration = 3f; // 공격 지속 시간
    [SerializeField] private float headRotationSpeed = 45f; // 머리 회전 속도 (도/초)
    [SerializeField] private float rotationAngleRange = 60f; // 회전 각도 범위 (좌우)
    
    [Header("Projectile Settings")]
    [SerializeField] private GameObject flameProjectilePrefab; // 화염 투사체 프리팹
    [SerializeField] private float projectileSpawnInterval = 0.1f; // 투사체 생성 간격
    [SerializeField] private float projectileSpeed = 10f; // 투사체 속도
    [SerializeField] private float projectileLifetime = 60f; // 투사체 수명
    
    [Header("References")]
    [SerializeField] private Transform headTransform; // 드래곤 머리 Transform
    [SerializeField] private Transform projectileSpawnPoint; // 투사체 발사 위치
    
    private Coroutine flameAttackRoutine;
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 2000;
        currentHealth = maxHealth;
        moveSpeed = 0.5f; // 드래곤 이동 속도
        attackDamage = 40;
        attackSpeed = 8f;
        experiencePoints = 500;
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
        
        // 이동 상태 진입 시 화염 브레스 루틴 시작
        StartFlameAttackRoutine();
    }
    
    public override void OnUpdateMove()
    {
        // 화염 브레스 공격 중에는 이동하지 않음
        if (isAttacking)
            return;
            
        base.OnUpdateMove();
    }
    
    public override void OnExitMove()
    {
        base.OnExitMove();
        
        // 이동 상태 종료 시 화염 브레스 루틴 중지
        StopFlameAttackRoutine();
    }
    
    // === Flame Breath Attack System ===
    private void StartFlameAttackRoutine()
    {
        if (flameAttackRoutine == null)
        {
            flameAttackRoutine = StartCoroutine(FlameAttackRoutine());
        }
    }
    
    private void StopFlameAttackRoutine()
    {
        if (flameAttackRoutine != null)
        {
            StopCoroutine(flameAttackRoutine);
            flameAttackRoutine = null;
        }
        isAttacking = false;
    }
    
    private IEnumerator FlameAttackRoutine()
    {
        while (true)
        {
            // 공격 주기까지 대기
            yield return new WaitForSeconds(flameAttackInterval);
            
            // 공격 시작
            isAttacking = true;
            
            // 화염 브레스 애니메이션 재생
            if (Animator != null)
            {
                Animator.SetTrigger(flameBreathAnimationTrigger);
            }
            
            // 화염 브레스 공격 코루틴 시작
            yield return StartCoroutine(PerformFlameBreath());
            
            // 공격 종료
            isAttacking = false;
        }
    }
    
    /// <summary>
    /// 화염 브레스 공격 수행: 드래곤이 바라보는 방향을 중심으로 머리를 회전시키며 투사체 발사
    /// </summary>
    private IEnumerator PerformFlameBreath()
    {
        if (headTransform == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("FireDragon: Head or SpawnPoint is null!");
            yield break;
        }
        
        float elapsedTime = 0f;
        float startAngle = -rotationAngleRange / 2f; // 시작 각도 (왼쪽)
        float endAngle = rotationAngleRange / 2f; // 끝 각도 (오른쪽)
        bool sweepingRight = true; // 회전 방향
        
        // 드래곤이 바라보는 방향을 기준으로 회전
        Quaternion baseRotation = transform.rotation;
        
        while (elapsedTime < attackDuration)
        {
            // 현재 회전 각도 계산 (수평 포물선 회전)
            float progress = (elapsedTime / attackDuration);
            float currentAngle;
            
            if (sweepingRight)
            {
                currentAngle = Mathf.Lerp(startAngle, endAngle, progress * 2f);
                if (progress >= 0.5f)
                {
                    sweepingRight = false;
                }
            }
            else
            {
                currentAngle = Mathf.Lerp(endAngle, startAngle, (progress - 0.5f) * 2f);
            }
            
            // 머리 회전 적용 (Y축 수평 회전)
            Quaternion rotationOffset = Quaternion.Euler(0f, currentAngle, 0f);
            headTransform.rotation = baseRotation * rotationOffset;
            
            // 화염 투사체 생성
            if (flameProjectilePrefab != null)
            {
                GameObject projectile = Instantiate(
                    flameProjectilePrefab, 
                    projectileSpawnPoint.position, 
                    headTransform.rotation
                );
                
                // 투사체 초기화
                FlameProjectile projectileScript = projectile.GetComponent<FlameProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.Initialize(attackDamage, projectileSpeed, projectileLifetime);
                }
                else
                {
                    // FlameProjectile 스크립트가 없으면 기본 Rigidbody로 발사
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = headTransform.forward * projectileSpeed;
                    }
                    Destroy(projectile, projectileLifetime);
                }
            }
            
            // 다음 투사체 생성까지 대기
            yield return new WaitForSeconds(projectileSpawnInterval);
            elapsedTime += projectileSpawnInterval;
        }
        
        // 머리를 원래 방향으로 복귀
        headTransform.rotation = baseRotation;
    }
}