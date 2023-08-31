using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : State
{
    PlayerController player;
    PlayerAvatar avatar;
    GameObject shieldCollider;
    public DefendState(PlayerController playerController) : base("Defend")
    {
        player = playerController;
        avatar = player.GetControlledAvatar();
    }

    public void SetShieldCollider(GameObject shieldCollider)
    {
        this.shieldCollider = shieldCollider;
        shieldCollider.SetActive(false);
    }

    public override void OnStateEnter()
    {
        avatar.Animator.SetBool("bIsDefending", true);
        shieldCollider.SetActive(true);
        base.OnStateEnter();
    }
    public override void OnStateExit()
    {
        avatar.Animator.SetBool("bIsDefending", false);
        shieldCollider.SetActive(false);
        base.OnStateExit();
    }
    public override void OnStateUpdate()
    {
        if(!player.ReadDefenseInput())
            player.StateMachine.ChangeState(player.IdleState);
        base.OnStateUpdate(); 
        
    }
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.GetControlledAvatar().onMove.Invoke(.1f);

    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
