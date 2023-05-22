using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : State
{
    MeleeCreatureController controller;
    float hurtDuration;
    public EnemyHurtState(MeleeCreatureController controller) : base("EnemyHurtState")
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
        hurtDuration -= Time.deltaTime;
        if(hurtDuration<=0)controller.stateMachine.ChangeState(controller.roamingState);
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}