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
        player.health.SetIgnoreDamage(true);
        player.myRigidbody.isKinematic = true;
        player.animator.SetBool("bDead", true);
        player.RemovePlayerControl();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        player.animator.SetBool("bDead", false);
        player.myRigidbody.isKinematic = false;
        player.health.SetIgnoreDamage(false);
        player.TryGivePlayerControl();
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
