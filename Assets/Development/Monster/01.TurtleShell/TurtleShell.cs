using UnityEngine;

public class TurtleShell : CommonMonsterBase
{
    protected override void Awake()
    {
        base.Awake();

        // maxHealth = 50;
        // currentHealth = maxHealth;
        // moveSpeed = 1.2f;
        // attackDamage = 2;
        // attackSpeed = 2f;
        // experiencePoints = 3;
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
}