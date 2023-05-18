using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : State
{
    PlayerController player;
    float movmentIntensity;
    public OnAirState(PlayerController playerController, float movmentIntensity=1) : base("OnAirState")
    {
        player = playerController;
        this.movmentIntensity =movmentIntensity;
    }

    public override void OnStateEnter()
    {
        player.animator.SetBool("bOnAir", true);
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        player.animator.SetBool("bOnAir", false);
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
        player.PlayerMovment(movmentIntensity);
        player.RotateBodyToFace();
        if(player.isGrounded())
            player.stateMachine.ChangeState(player.idleState);
        
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
