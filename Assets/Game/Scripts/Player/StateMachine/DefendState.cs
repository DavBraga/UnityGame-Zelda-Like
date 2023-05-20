using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    PlayerController player;
    GameObject shieldCollider;
    public DefendState(PlayerController playerController, GameObject shieldCollider) : base("Defend")
    {
        player = playerController;
        this.shieldCollider = shieldCollider;
    }

    public override void OnStateEnter()
    {
        player.animator.SetBool("bIsDefending", true);
        shieldCollider.SetActive(true);
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        player.animator.SetBool("bIsDefending", false);
        shieldCollider.SetActive(false);
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        if(Input.GetMouseButtonUp(1))
            player.stateMachine.ChangeState(player.idleState);
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
