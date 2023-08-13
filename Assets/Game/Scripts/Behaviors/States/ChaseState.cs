using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : State
{
    CreatureController controller;
    float chaseDuration=0;
    State meleeState;
    State fallBackState;
    State rangedState;
    Vector3 targetPosition;
    float lastRangedState =0;
    public ChaseState(CreatureController controller) : base("ChaseState")
    {
        this.controller =controller;
    }
    public void  SetUpState(State combatState, State fallBackState, State rangedState )
    {
        this.meleeState = combatState;
        this.fallBackState = fallBackState;
        this.rangedState = rangedState;
    }
    public void  SetUpState(State combatState, State fallBackState)
    {
        this.meleeState = combatState;
        this.fallBackState = fallBackState;
        this.rangedState = this.fallBackState;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        if(controller.myNavAgent.enabled)
        controller.myNavAgent.isStopped =false; 
        if(GameManager.Instance)
        controller.gameObject.transform.LookAt(GameManager.Instance.GetPlayer().transform);
        
        chaseDuration=controller.ceaseFollowThreshold;
        targetPosition = GameManager.Instance.GetPlayer().transform.position;
        controller.myNavAgent.SetDestination(targetPosition);
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        targetPosition = GameManager.Instance.GetPlayer().transform.position;
       
        bool rangeCheckResult = RangeCheck(targetPosition);
        bool SightCheckResult = SightCheck(targetPosition);

        if (rangeCheckResult && SightCheckResult)
        {
            controller.stateMachine.ChangeState(meleeState);
            return;
        } 
        if(controller.rangedOverMelee&& Time.time>lastRangedState+ controller.rangedStateIntervals)
        {
            if(SightCheck(targetPosition))
            {
                lastRangedState = Time.time;
                controller.stateMachine.ChangeState(rangedState);
                return;
            } 
        }
        //set destination
        if(controller.myNavAgent.enabled)
        controller.myNavAgent.SetDestination(targetPosition);
        // cease if too far away
        if (!CreatureHelper.IstargetInRange(controller.sightRange, controller.transform.position, GameManager.Instance.GetPlayer().transform.position))
        {
            //todo
            controller.stateMachine.ChangeState(fallBackState);
            return;
        }

        //after a while chasing do something else
        if ((chaseDuration -= Time.deltaTime) < 0)
        {
            if (!SightCheckResult)
            {
                controller.stateMachine.ChangeState(fallBackState);
                return;
            }
            //change state based on range  
            if (rangeCheckResult)
            {
                controller.stateMachine.ChangeState(meleeState);
            }
            else controller.stateMachine.ChangeState(rangedState);
        }
    }
    private bool RangeCheck(Vector3 targetPosition)
    {
        return CreatureHelper.IstargetInRange(controller.meleeRange,
            controller.transform.position, targetPosition);
    }
    private bool SightCheck(Vector3 targetPosition)
    {
       return CreatureHelper.IsTargetOnSight(targetPosition,
            controller.transform,controller.sightRange);
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}