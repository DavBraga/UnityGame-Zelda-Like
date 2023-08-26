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
        player.forceNormalGravity = true;
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        player.animator.SetBool("bOnAir", false);
        player.animator.ResetTrigger("tJump");
        player.forceNormalGravity = false;
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
       
        if(player.isGroundedDelegate())
            player.stateMachine.ChangeState(player.idleState);
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.onMove.Invoke(movmentIntensity);
        player.onRotate.Invoke(movmentIntensity);

    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
