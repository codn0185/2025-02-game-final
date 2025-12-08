using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mushroom : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 10;
        currentHealth = maxHealth;
        moveSpeed = 3.0f;
        attackDamage = 1;
        attackSpeed = 0.8f;
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
