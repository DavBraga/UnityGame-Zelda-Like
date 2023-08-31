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
        if(!player.GetControlledAvatar().isGroundedDelegate())
        {
            player.StateMachine.ChangeState(player.OnAirState);
        }
        if(!player.inputMovmentVector.isZero())
        {
            player.StateMachine.ChangeState(player.WalkingState);
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
