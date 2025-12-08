using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swarm09 : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 50;
        currentHealth = maxHealth;
        moveSpeed = 2.8f;
        attackDamage = 7;
        attackSpeed = 1.5f;
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
