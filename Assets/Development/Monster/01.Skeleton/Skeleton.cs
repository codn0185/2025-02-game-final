using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : CommonMonsterBase
{

    protected override void Awake()
    {
        base.Awake();
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
