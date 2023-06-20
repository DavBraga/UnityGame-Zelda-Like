using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : State
{
    CreatureController controller;
    float chaseDuration=0;
    State combatState;
    State fallBackState;
    State rangedState;
    public ChaseState(CreatureController controller) : base("ChaseState")
    {
        this.controller =controller;
    }

    public void  SetUpState(State combatState, State fallBackState, State rangedState )
    {
        this.combatState = combatState;
        this.fallBackState = fallBackState;
        this.rangedState = rangedState;
    }

       public void  SetUpState(State combatState, State fallBackState)
    {
        this.combatState = combatState;
        this.fallBackState = fallBackState;
        this.rangedState = this.fallBackState;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
        controller.myNavAgent.isStopped =false;
        if(GameManager.Instance)
        controller.gameObject.transform.LookAt(GameManager.Instance.GetPlayer().transform);
        
        chaseDuration=controller.ceaseFollowThreshold;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Vector3 targetPosition = GameManager.Instance.GetPlayer().transform.position;
        //set destination
        controller.myNavAgent.SetDestination(targetPosition);
        // cease if too far away
        if(!CreatureHelper.IstargetInRange(controller.sightRange, controller.transform.position,GameManager.Instance.GetPlayer().transform.position))
        {
            //todo
            controller.stateMachine.ChangeState(fallBackState);
            return;
        }
        
        if((chaseDuration-=Time.deltaTime)<0)
        {
            //if cant see anymore give up
            bool SightCheckResult = CreatureHelper.IsTargetOnSight(
                GameManager.Instance.GetPlayer().transform.position,
                controller.transform.position, 
                controller.sightRange);

            if(!SightCheckResult)
            {
                controller.stateMachine.ChangeState(fallBackState);
                return;
            }
                
            //swap to attack if in attackrange
            bool rangeCheckResult =  
                CreatureHelper.IstargetInRange(controller.baseAttack.AttackRadius,
                controller.transform.position,targetPosition);
                
            if(rangeCheckResult)
            {
                controller.stateMachine.ChangeState(combatState);
            }
            else    controller.stateMachine.ChangeState(rangedState);  
        }
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}