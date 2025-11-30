using System.Collections;
using UnityEngine;

public class PlayerController : Controller<PlayerFSM>
{
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }

    public float moveSpeed = 5.0f;
    public float attackSpeed = 2.0f;

    private Coroutine attackCoroutine;

    protected void Awake()
    {
        StateMachine = new PlayerFSM(this);
        
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        StateMachine.Initialize(StateMachine.IdleState);
    }

    protected void Update()
    {
    }

    public void StartMove()
    {
        Animator.SetBool(PlayerAnimatorParameter.Move, true);
    }

    public void StopMove()
    {
        Animator.SetBool(PlayerAnimatorParameter.Move, false);
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new(moveSpeed * Time.deltaTime * horizontal, 0, 0);
        transform.Translate(movement);
    }

    public void StartAttackCoroutine()
    {
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    public void StopAttackCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            Attack();
        }
    }

    private void Attack()
    {
        Animator.SetTrigger(PlayerAnimatorParameter.Attack);
    }

    public void TakeDamage()
    {
        Animator.SetTrigger(PlayerAnimatorParameter.Hit);
    }

    public bool HasMovementInput()
    {
        return Input.GetAxis("Horizontal") != 0;
    }

    public bool IsDead()
    {
        // 사망 조건 구현
        return false;
    }
}