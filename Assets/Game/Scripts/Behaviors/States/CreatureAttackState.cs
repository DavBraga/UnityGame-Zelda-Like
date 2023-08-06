using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackState : State
{
    CreatureController controller;

    Coroutine attackUndergoing;

    Attack_SO defaultAttack;

    Coroutine attackExecution;
    Attack_SO usedAttack;
    State fallBackState;
    public CreatureAttackState(CreatureController controller) : base("CreatureAttackState")
    {
        this.controller =controller;
    }
    public void SetUpState(State fallBackState, Attack_SO defaultAttack)
    {
        this.fallBackState = fallBackState;
        this.defaultAttack = defaultAttack;
        usedAttack = defaultAttack;
    }

    public void SetAttack(Attack_SO attackToUse)
    {
        usedAttack = attackToUse;
    }
    public void ResetAttack()
    {
        usedAttack = defaultAttack;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        if (attackUndergoing != null)
        {      
            controller.StopCoroutine(attackUndergoing);
        }
        attackUndergoing = controller.StartCoroutine(WaitAttackCompletion());
       
        // if (attackExecution != null)
        // {
            
        //     controller.StopCoroutine(attackExecution);
        // }
        // attackExecution = controller.StartAttack(usedAttack);
    }

    IEnumerator WaitAttackCompletion()
    {
        if (attackExecution != null)
        {      
            controller.StopCoroutine(attackExecution);
        }
        yield return attackExecution = controller.StartAttack(usedAttack);
        controller.stateMachine.ChangeState(fallBackState);
    }

    private void ExecuteAttack()
    {
        controller.HaltMovment();
        if (attackExecution != null)
        {
            
            controller.StopCoroutine(attackExecution);
        }
        attackExecution = controller.StartAttack(usedAttack);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        controller.StopCoroutine(attackExecution);
        controller.StopCoroutine(attackUndergoing);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
         //fall back after duration elapsed
        // if(( isWarmingUp &&(warmup-=Time.deltaTime)<0))
        // {
        //     ExecuteAttack();
        //     isWarmingUp = false;
        // }
        
        // if((attackDuration-=Time.deltaTime)<0)
        // {
        //     controller.stateMachine.ChangeState(fallBackState);
        //     return;
        // } 
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}
