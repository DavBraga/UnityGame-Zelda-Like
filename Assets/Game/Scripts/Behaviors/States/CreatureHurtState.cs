using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHurtState : State
{
    CreatureController controller;
    float hurtDuration;
    float imunityDuration;
    State fallBackState;
    public CreatureHurtState(CreatureController controller) : base("CreatureHurt")
    {
        this.controller = controller;
    }

     public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log(" enemey hurt state");
        controller.myHealth.SetIgnoreDamage(true);
        controller.myNavAgent.isStopped = true;
        hurtDuration = controller.hurtDuration;
        imunityDuration =Time.time+ controller.imunityDuration;
        
    }

    public void SetUpState(State fallBackState)
    {
        this.fallBackState =fallBackState;
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        controller.myHealth.SetIgnoreDamage(false);
        
    }

      public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        hurtDuration -= Time.deltaTime;
        if(Time.time>imunityDuration)controller.myHealth.SetIgnoreDamage(false);
        if(hurtDuration<=0)
            controller.stateMachine.ChangeState(fallBackState);             

    }
       public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
