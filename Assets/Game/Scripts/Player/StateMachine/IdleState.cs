using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    PlayerController player;
    public IdleState(PlayerController playerController) : base("Idle")
    {
        player = playerController;
    }

    public override void OnStateEnter()
    {
        if(player.attacking) player.KeepChooping();
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
        if(!player.IsGrounded())
        {
            player.stateMachine.ChangeState(player.onAirState);
        }
        if(!player.inputMovmentVector.isZero())
        {
            player.stateMachine.ChangeState(player.walkingState);
        }
       // if(player.ReadSpaceBarInput()) player.Jump();
        
        
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
