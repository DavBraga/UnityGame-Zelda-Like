using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : State
{
    PlayerController player;
    PlayerAvatar avatar;
    float movmentIntensity;
    public OnAirState(PlayerController playerController, float movmentIntensity=1) : base("OnAirState")
    {
        player = playerController;
        avatar = player.GetControlledAvatar();
        this.movmentIntensity =movmentIntensity;
    }

    public void SetAirMovmentModifier(float value)
    {
        this.movmentIntensity = value;
    }

    public override void OnStateEnter()
    {
        avatar.animator.SetBool("bOnAir", true);
        
        player.GetControlledAvatar().onAir.Invoke();
        //player.forceNormalGravity = true;
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        avatar.animator.SetBool("bOnAir", false);
        avatar.animator.ResetTrigger("tJump");
        player.GetControlledAvatar().onLand.Invoke();
        //player.forceNormalGravity = false;
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
       
        if(player.GetControlledAvatar().isGroundedDelegate())
            player.stateMachine.ChangeState(player.idleState);
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.GetControlledAvatar().onMove.Invoke(movmentIntensity);
        player.GetControlledAvatar().onRotate.Invoke(movmentIntensity);

    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
