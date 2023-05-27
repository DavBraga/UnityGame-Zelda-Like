using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingState : State
{
    // search for target every SearchInterval seconds;
    // if target found change to alertState
    MeleeCreatureController controller;
    float searchCooldown;
    public RoamingState(MeleeCreatureController controller) : base("Roaming")
    {
        this.controller =controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
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
        // do nothing if not in range
        if(!controller.helper.IstargetInRange(controller.searchRadius)) return;
        //if can see go alert state
        if(controller.helper.IsTargetOnSight())
        {
            controller.stateMachine.ChangeState(controller.alertState);
        }
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}