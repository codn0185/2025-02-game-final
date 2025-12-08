using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swarm08 : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 40;
        currentHealth = maxHealth;
        moveSpeed = 2.3f;
        attackDamage = 8;
        attackSpeed = 1.2f;
        experiencePoints = 15;
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
