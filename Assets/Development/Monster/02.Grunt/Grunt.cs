using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grunt : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 100;
        currentHealth = maxHealth;
        moveSpeed = 1.5f;
        attackDamage = 10;
        attackSpeed = 1.8f;
        experiencePoints = 20;
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
