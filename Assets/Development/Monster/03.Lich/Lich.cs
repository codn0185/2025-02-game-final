using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : CommonMonsterBase
{
    protected override void Awake()
    {
        base.Awake();

        maxHealth = 500;
        currentHealth = maxHealth;
        moveSpeed = 2.0f;
        attackDamage = 20;
        attackSpeed = 0.7f;
        experiencePoints = 80;
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
