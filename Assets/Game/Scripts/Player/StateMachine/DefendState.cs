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
        player.LimitMovmentSpeed(.1f);
        player.PlayerMovment();
        
        //player.RotateBodyToFace(.03f);
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }

    public bool CanInflictDamage(GameObject attacker, int Damage)
    {
        Vector3 playerDirection = player.transform.TransformDirection(Vector3.forward);
        Vector3 attackerDirection = (player.transform.position- attacker.transform.position).normalized;
        float dot = Vector3.Dot(playerDirection, attackerDirection);
        Debug.Log(dot);
        if(dot<-.25f) return false;
        
        return true;
    }
}
