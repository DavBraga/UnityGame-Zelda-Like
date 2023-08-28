using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    PlayerAvatar player;
    GameObject shieldCollider;
    public DefendState(PlayerAvatar playerController) : base("Defend")
    {
        player = playerController;
    }

    public void SetShieldCollider(GameObject shieldCollider)
    {
        this.shieldCollider = shieldCollider;
        shieldCollider.SetActive(false);
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
        if(!player.ReadDefenseInput())
            player.stateMachine.ChangeState(player.idleState);
        base.OnStateUpdate(); 
        
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.onMove.Invoke(.1f);

    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
