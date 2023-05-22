using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{
    MeleeCreatureController controller;
    public EnemyAttackState(MeleeCreatureController controller) : base("EnemyAttackState")
    {
        this.controller =controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}