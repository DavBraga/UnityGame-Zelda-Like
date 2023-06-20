using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDeadState : State
{
    CreatureController controller;

    public CreatureDeadState(CreatureController controller): base("CreatureDeadState")
    {
        this.controller = controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Dead state");
        controller.myHealth.SetIgnoreDamage(true);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Debug.Log("Leaves Dead state");
        controller.myHealth.SetIgnoreDamage(false);
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
