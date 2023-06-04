using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : State
{
    MeleeCreatureController controller;
    public EnemyDeadState(MeleeCreatureController controller) : base("EnemyDeadState")
    {
        this.controller =controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Dead state");
        controller.health.SetIgnoreDamage(true);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
         Debug.Log("Leaves Dead state");
         
        controller.health.SetIgnoreDamage(false);
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