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
        Debug.Log(" enemey hurt state");
        controller.health.SetIgnoreDamage(true);
        controller.myNavAgent.isStopped = true;
        hurtDuration = controller.hurtDuration;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        
        controller.health.SetIgnoreDamage(false);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        hurtDuration -= Time.deltaTime;
        if(hurtDuration<=0)
            {
                if(controller.health.GetCurrentHealth()<=0)controller.Die();
                else controller.stateMachine.ChangeState(controller.roamingState);
                
            }
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}