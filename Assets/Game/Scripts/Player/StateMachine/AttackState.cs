using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    PlayerController player;

    Coroutine routine;
    GameObject attackCollider;
    int attackStage = -1; 

    float attackChainWindow= 2.1f;
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
        SetVariables();
        EvolveAttackStages();
       
       
       
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
        if(GameManager.Instance.GameState!=GameState.playing) player.stateMachine.ChangeState(player.idleState);
        base.OnStateUpdate();
        stageRemainingDuration -=Time.deltaTime;

        

        if(stageRemainingDuration<0){
             player.stateMachine.ChangeState(player.idleState);
            return;
        }  
       if(stageRemainingDuration>attackChainWindow) return;
        if(attackStage>1)
        {
            
           player.stateMachine.ChangeState(player.idleState);
            return;
        }
        if(player.ReadAttackInput())
        {
            stageRemainingDuration =500f;
            
            
            EvolveAttackStages();
            player.PlayAttackAnimation(attackStage);

            SetVariables();
        }

        
    } 
    public void SetVariables()
    {
        float attackDuration = player.GetAttackPreparationTIme()[attackStage];
        stageRemainingDuration = attackDuration + attackChainWindow;

    }

    public void EvolveAttackStages()
    {
        attackStage ++;
        player.attackstage = attackStage;
        applyImpulse = true;
        attackCollider.SetActive(true);
        Debug.Log("Went stage:"+ attackStage);
        player.mySFXManager.PlayAudio();

    }   
    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.RotateBodyToFace(1);

        if(stageRemainingDuration<attackChainWindow)
            attackCollider.SetActive(false);

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
