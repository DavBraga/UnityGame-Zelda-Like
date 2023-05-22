using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : State
{
    MeleeCreatureController controller;
    public AlertState(MeleeCreatureController controller) : base("Alert")
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
        //move to target if in range
        // cease if too far away
        //swap to attack if in attackrange
        base.OnStateUpdate();
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}