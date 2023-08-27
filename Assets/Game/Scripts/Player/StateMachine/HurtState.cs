using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState: State
{
    PlayerController player;
    float hurtDuration;
    public HurtState(PlayerController playerController) : base("HurtState")
    {
        player = playerController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        player.animator.SetTrigger("tHurt");
        hurtDuration = player.hurtDuration;
        player.RemovePlayerControl();
        Time.timeScale = .7f;
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        player.TryGivePlayerControl();
         Time.timeScale = 1f;
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
        if((hurtDuration -=Time.deltaTime)<0)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
        
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
