using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MonsterController : Controller<MonsterFSM>
{
    private const float DESTROY_DELAY = 2.0f;
    public static readonly HashSet<MonsterController> Entities = new HashSet<MonsterController>();

    protected int maxHealth;
    protected int currentHealth;
    protected float moveSpeed;
    protected int attackDamage;
    protected float attackSpeed;
    protected int experiencePoints;

    private Coroutine attackCoroutine;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public Slider healthBar;

    // === Unity Lifecycle ===

    protected virtual void Awake()
    {
        StateMachine = new MonsterFSM(this);

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        healthBar.gameObject.SetActive(false);
        Entities.Add(this);

    }

    protected virtual void Start()
    {
        StateMachine.Initialize(StateMachine.IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.Update();
    }

    // === Monster Actions ===

    public virtual void Move()
    {
        Rigidbody.MovePosition(transform.position + moveSpeed * Time.deltaTime * transform.forward);
    }

    public virtual void Attack()
    {
        Animator.SetTrigger(MonsterAnimatorParameter.Attack.ToString());
        // GameManager.instance.DecreaseHP(attackDamage);
    }

    public virtual void Die()
    {
        GetComponent<Collider>().enabled = false;
        healthBar.gameObject.SetActive(false);
        GameProgressManager.Instance.AddExperience(experiencePoints);
        Destroy(gameObject, DESTROY_DELAY);
    }

    public virtual void TakeDamage(int damage)
    {
        if (StateMachine.CurrentState == StateMachine.DeadState) return;
        Animator.SetTrigger(MonsterAnimatorParameter.Hit);
        currentHealth -= damage;
        UpdateHealthBar();
        healthBar.gameObject.SetActive(true);
        if (currentHealth <= 0)
        {
            StateMachine.ChangeState(StateMachine.DeadState);
        }
    }

    public void StartAttackCoroutine()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public void StopAttackCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    // === Helper Methods ===

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            Attack();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // === Trigger Events ===

    protected void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(Tag.AttackPoint))
        {
            StateMachine.ChangeState(StateMachine.BattleState);
        }

    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.AttackPoint))
        {
            StateMachine.ChangeState(StateMachine.MoveState);
        }
    }


}