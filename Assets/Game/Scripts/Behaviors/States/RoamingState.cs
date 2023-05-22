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

        if(searchCooldown>0) return;
        searchCooldown = controller.searchInterval;
        if(!controller.helper.IstargetInRange()) return;

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