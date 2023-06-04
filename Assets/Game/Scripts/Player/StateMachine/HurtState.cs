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
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
        if((hurtDuration -=Time.deltaTime)<0)
        {
            if(player.GetCurrentHealth()<=0)
                {
                    Debug.Log("dies");
                    player.Die();
                }
            else
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
