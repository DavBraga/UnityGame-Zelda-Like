using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    PlayerAvatar player;
    public IdleState(PlayerAvatar playerController) : base("Idle")
    {
        player = playerController;
    }

    public override void OnStateEnter()
    {
        if(player.attacking) player.onAttack.Invoke();
        if(player.defending) player.onDefend.Invoke();
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate(); 
        if(!player.isGroundedDelegate())
        {
            player.stateMachine.ChangeState(player.onAirState);
        }
        if(!player.inputMovmentVector.isZero())
        {
            player.stateMachine.ChangeState(player.walkingState);
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
