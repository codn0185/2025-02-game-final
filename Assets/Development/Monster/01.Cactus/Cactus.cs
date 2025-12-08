using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cactus : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 15;
        currentHealth = maxHealth;
        moveSpeed = 2.0f;
        attackDamage = 2;
        attackSpeed = 1.5f;
        experiencePoints = 1;
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
