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
       EvolveAttackStages();
       SetVariables();
       player.animator.SetBool("bIsAttacking", true);
       
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
            player.stateMachine.BackToLastState();
            return;
        }
        if(player.ReadLeftMouseInput())
        {
            stageRemainingDuration =500f;
            
            
            EvolveAttackStages();
            player.PlayAttackAnimation(attackStage);

            SetVariables();
        }

        
    } 
    public void SetVariables()
    {
        float attackDuration = player.GetAttackDuration()[attackStage];
        attackChainWindow = player.getAttackChainWindow()[attackStage];
        stageRemainingDuration = attackDuration + attackChainWindow;

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

        if(stageRemainingDuration<attackChainWindow)
            attackCollider.SetActive(false);

        if(applyImpulse)
        {
            attackCollider.SetActive(true);
            player.PlayAttackImpulse(attackStage);
            applyImpulse = false;
        }
    }
            
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
