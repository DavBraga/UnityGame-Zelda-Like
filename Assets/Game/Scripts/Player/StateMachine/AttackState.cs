using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    PlayerController player;
    GameObject attackCollider;
    int attackStage = -1; 

    float attackChainWindow;
    float stageRemainingDuration;

    bool applyImpulse = false;

    float maxDuration;
    public AttackState(PlayerController playerController, GameObject attackCollider) : base("Attack")
    {
        player = playerController;
        this.attackCollider = attackCollider;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
       attackCollider.SetActive(true);
       EvolveAttackStages();
       SetVariables();
       player.animator.SetBool("bIsAttacking", true);
       
       //player.PlayAttackAnimation(attackStage);
       
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
       attackCollider.SetActive(false);
       player.attackstage = attackStage = -1;
       player.animator.SetBool("bIsAttacking", false);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        stageRemainingDuration -=Time.deltaTime;

        if(stageRemainingDuration<0){
             player.stateMachine.BackToLastState();
            return;
        }  
        if(stageRemainingDuration>attackChainWindow) return;
        if(attackStage>1)
        {
            // player.attackstage = attackStage =0;
            // Debug.Log("Went stage:"+ attackStage);
            // SetVariables();
            player.stateMachine.BackToLastState();
            return;
        }
        if(player.ReadAttackInput())
        {
            stageRemainingDuration =500f;
            
            
            EvolveAttackStages();
            player.PlayAttackAnimation(attackStage);
            // if(attackStage==2)
            // player.myRigidbody.AddForce(player.transform.forward *10,ForceMode.Impulse);
            SetVariables();
        }

        
    } 
    public void SetVariables()
    {
        float attackDuration = player.GetAttackDuration()[attackStage];
        attackChainWindow = player.getAttackChainWindow()[attackStage];
        stageRemainingDuration = attackDuration + attackChainWindow;
        
        //attackInterval= player.GetAttackChainDelay()[attackStage];
        //attackCooldown = stageDuration*attackInterval;
    }

    public void EvolveAttackStages()
    {
        attackStage ++;
        player.attackstage = attackStage;
        applyImpulse = true;
        Debug.Log("Went stage:"+ attackStage);

    }   
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.RotateBodyToFace(1);

        if(applyImpulse)
        {
            player.PlayAttackImpulse(attackStage);
            applyImpulse = false;
        }
    }
            
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
