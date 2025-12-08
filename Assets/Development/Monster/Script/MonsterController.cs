using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 몬스터 컨트롤러 기본 클래스
/// 모든 몬스터는 이 클래스를 상속하여 구현
/// </summary>
public abstract class MonsterController : Controller<MonsterFSM>
{
    // === Constants ===
    private const float DESTROY_DELAY = 5.0f;

    // === Static Collections ===
    // 현재 활성화된 모든 몬스터 인스턴스
    public static readonly HashSet<MonsterController> Entities = new();

    // === Serialized Fields ===
    [Header("Debug Info")]
    [SerializeField] private MonsterStateType currentStateType;
    [SerializeField] private bool ForceStateChange = false;

    [Header("Monster Stats")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float baseMoveSpeed;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int experiencePoints;
    [SerializeField] protected int dropCoins;
    [SerializeField] protected int dropGems;
    [Header("UI")]
    // [SerializeField] private Slider healthBar;
    // === Private Fields ===
    private Coroutine attackCoroutine;

    // === Public Properties ===
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Slider healthBar;
    public AudioClip hitAudio;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsReachedAttackPoint { get; set; } = false;
    public bool IsDead { get; private set; } = false;

    // === Unity Lifecycle ===

    protected virtual void Awake()
    {
        StateMachine = new MonsterFSM(this);

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        healthBar.gameObject.SetActive(false);
        Entities.Add(this);
        moveSpeed = baseMoveSpeed;
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        Initialize(StateMachine.IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.Update();
    }

    protected virtual void LateUpdate()
    {
        if (StateMachine.CurrentState == StateMachine.MoveState)
            Rigidbody.MovePosition(transform.position + moveSpeed * Time.deltaTime * transform.forward);
    }

    public void TakeDamage(int damage)
    {
        if (StateMachine.CurrentState == StateMachine.DeadState) return;
        Animator.SetTrigger(MonsterAnimatorParameter.Hit);
        currentHealth -= damage;
        UpdateHealthBar();
        healthBar.gameObject.SetActive(true);
        if (currentHealth <= 0)
            IsDead = true;
        if (ForceStateChange && currentStateType != StateMachine.CurrentState.StateType)
        {
            ChangeStateByType(currentStateType);
            ForceStateChange = false;
        }
        else
        {
            currentStateType = StateMachine.CurrentState.StateType;
        }
    }

    // === Trigger Events ===
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.AttackPoint))
        {
            IsReachedAttackPoint = true;
        }
        else if (other.CompareTag(Tag.Bullet))
            OnHit(other);
    }

    protected void OnHit(Collider other)
    {
        if (other.CompareTag(Tag.Bullet))
        {
            if (IsDead)
            {
                return;
            }

            Bullet bullet = other.GetComponentInParent<Bullet>();

            TakeDamage(bullet.damage);

            bullet.OnHit();

            if (bullet.isChain)
            {
                ApplyChain(bullet.chainCount - 1, bullet, this);
            }

        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.AttackPoint))
        {
            IsReachedAttackPoint = false;
        }
    }

    // === State Behavior Methods ===
    public virtual void OnEnterIdle()
    {
        Animator.SetBool(MonsterAnimatorParameter.Idle, true);
    }

    public virtual void OnUpdateIdle()
    {
        if (IsDead)
            StateMachine.ChangeState(StateMachine.DeadState);
    }

    public virtual void OnExitIdle()
    {
        Animator.SetBool(MonsterAnimatorParameter.Idle, false);
    }

    public virtual void OnEnterMove()
    {
        Animator.SetBool(MonsterAnimatorParameter.Move, true);
    }

    public virtual void OnUpdateMove()
    {
        if (IsDead)
            StateMachine.ChangeState(StateMachine.DeadState);
        else if (IsReachedAttackPoint)
            StateMachine.ChangeState(StateMachine.BattleState);
        else
            Move();
    }

    public virtual void OnExitMove()
    {
        Animator.SetBool(MonsterAnimatorParameter.Move, false);
    }

    public virtual void OnEnterBattle()
    {
        Animator.SetBool(MonsterAnimatorParameter.Battle, true);
        BeginAttackRoutine();
    }

    public virtual void OnUpdateBattle()
    {
        if (IsDead)
            StateMachine.ChangeState(StateMachine.DeadState);
        else if (!IsReachedAttackPoint)
            StateMachine.ChangeState(StateMachine.MoveState);
    }

    public virtual void OnExitBattle()
    {
        Animator.SetBool(MonsterAnimatorParameter.Battle, false);
        EndAttackRoutine();
    }

    public virtual void OnEnterDead()
    {
        Animator.SetBool(MonsterAnimatorParameter.Dead, true);
        Die();
    }

    public virtual void OnUpdateDead()
    {
    }

    public virtual void OnExitDead()
    {
        Animator.SetBool(MonsterAnimatorParameter.Dead, false);
    }

    // === Monster Actions ===
    protected virtual void Move()
    {
        Vector3 movement = transform.forward * (moveSpeed * Time.deltaTime);
        Rigidbody.MovePosition(transform.position + movement);
    }

    protected virtual void Attack()
    {
        Animator.SetTrigger(MonsterAnimatorParameter.Attack.ToString());
        // GameProgressManager.Instance.DealDamageToPlayer(attackDamage);
    }

    protected virtual void Die()
    {
        GetComponent<Collider>().enabled = false;
        healthBar.gameObject.SetActive(false);
        Animator.SetBool(MonsterAnimatorParameter.Dead, true);
        SoundManager.instance.PlayAudio(hitAudio);
        GameProgressManager.Instance.AddExperience(experiencePoints);
        GameProgressManager.Instance.AddCoins(dropCoins);
        GameProgressManager.Instance.AddGems(dropGems);
        GameProgressManager.Instance.AddKillCount();
        Entities.Remove(this);
        moveSpeed = 0;
        Destroy(gameObject, DESTROY_DELAY);
    }

    private void BeginAttackRoutine()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    private void EndAttackRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            Attack();
        }
    }

    // === Helper Methods ===
    private void Initialize(MonsterState initialState)
    {
        StateMachine.Initialize(initialState);
        currentStateType = initialState.StateType;
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // 인스펙터의MonsterStateType을 기반으로 FSM 상태를 변경
    private void ChangeStateByType(MonsterStateType stateType)
    {
        MonsterState targetState = stateType switch
        {
            MonsterStateType.Idle => StateMachine.IdleState,
            MonsterStateType.Move => StateMachine.MoveState,
            MonsterStateType.Battle => StateMachine.BattleState,
            MonsterStateType.Dead => StateMachine.DeadState,
            _ => StateMachine.IdleState
        };

        StateMachine.ChangeState(targetState);
    }

    public void ApplyChain(int chainCount, Bullet bullet, MonsterController avoid)
    {
        StartCoroutine(ChainCoroutine(chainCount, bullet, avoid));
    }

    IEnumerator ChainCoroutine(int chainCount, Bullet bullet, MonsterController avoid)
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 pos = transform.position;
        MonsterController closestEnemy = this;
        float closest = float.PositiveInfinity;

        foreach (MonsterController enemy in Entities)
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
}