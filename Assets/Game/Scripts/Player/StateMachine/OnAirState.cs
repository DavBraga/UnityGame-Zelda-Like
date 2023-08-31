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
        avatar.Animator.SetBool("bOnAir", true);
        
        player.GetControlledAvatar().onAir.Invoke();
        //player.forceNormalGravity = true;
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        avatar.Animator.SetBool("bOnAir", false);
        avatar.Animator.ResetTrigger("tJump");
        player.GetControlledAvatar().onLand.Invoke();
        //player.forceNormalGravity = false;
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
       
        if(player.GetControlledAvatar().isGroundedDelegate())
            player.StateMachine.ChangeState(player.IdleState);
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
