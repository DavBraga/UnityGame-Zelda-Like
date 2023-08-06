using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRoamingState : State
{
    CreatureController controller;
    float searchCooldown;

    State onSightState;
    public CreatureRoamingState(CreatureController controller) : base("Roaming State")
    {
        this.controller = controller;
    }
    public void SetUpState(State onSightState)
    {
        this.onSightState = onSightState;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        controller.myNavAgent.isStopped = false;
        searchCooldown = controller.searchInterval;
    }
     public override void OnStateExit()
    {
        base.OnStateExit();
    }

      public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        searchCooldown -= Time.deltaTime;
        //wait for cooldown
        if(searchCooldown>0) return;
        searchCooldown = controller.searchInterval;
        Vector3 targetPosition = GameManager.Instance.GetPlayer().transform.position;

        bool rangeCheckResult = CreatureHelper.IstargetInRange(controller.searchRadius,controller.transform.position, targetPosition);
        // do nothing if not in range
        if(!rangeCheckResult) return;

        // hear if too close and ignore sight check
         if (CreatureHelper.CanIHearMyTarget(GameManager.Instance.GetPlayer().transform.position,controller.transform,controller.hearRange))
         {
            controller.stateMachine.ChangeState(onSightState);
            return;
         }
        //if can see go alert state
        bool SightCheckResult = CreatureHelper.IsTargetOnSight(targetPosition,controller.transform,controller.sightRange);
        if(SightCheckResult)
        {
            controller.stateMachine.ChangeState(onSightState);
        }
    }
}
