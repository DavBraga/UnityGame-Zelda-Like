using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    MeleeCreatureController controller;
    float ceaseFollowCooldown=0;
    public AlertState(MeleeCreatureController controller) : base("Alert")
    {
        this.controller =controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
        controller.myNavAgent.isStopped = false;
        ceaseFollowCooldown=controller.ceaseFollowThreshold;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void OnStateUpdate()
    { 
        base.OnStateUpdate();
        //set destination
        controller.myNavAgent.SetDestination(GameManager.Instance.GetPlayer().transform.position);
        // cease if too far away
        if(!controller.helper.IstargetInRange(controller.sightRange))
        {
            controller.stateMachine.ChangeState(controller.roamingState);
        }
        //if cant see anymore give up
        if((ceaseFollowCooldown-=Time.deltaTime)<0)
        {      
            if(controller.helper.IsTargetOnSight()) 
                ceaseFollowCooldown = controller.ceaseFollowThreshold;
            else
                controller.stateMachine.ChangeState(controller.roamingState);
        }
        //swap to attack if in attackrange
        if(controller.helper.IstargetInRange(controller.attackRadius))
        {
            controller.stateMachine.ChangeState(controller.enemyAttackState);
        }
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}