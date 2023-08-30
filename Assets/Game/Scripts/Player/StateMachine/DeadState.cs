using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    PlayerController player;
    public DeadState(PlayerController playerController) : base("Idle")
    {
        player = playerController;
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
        base.OnStateUpdate(); 

        
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
