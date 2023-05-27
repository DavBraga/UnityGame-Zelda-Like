using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{
    MeleeCreatureController controller;
    float attackDuration=0;
    IEnumerator attackCoroutine;
    public EnemyAttackState(MeleeCreatureController controller) : base("EnemyAttackState")
    {
        this.controller =controller;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        attackDuration = controller.attackDuration;
        controller.StartCoroutine(attackCoroutine=StartAttack());
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        //cancel
        if(attackCoroutine!=null) controller.StopCoroutine(attackCoroutine);
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        // alert if its not in attackrange
        if(!controller.helper.IstargetInRange(controller.attackRadius))
        {
            controller.stateMachine.ChangeState(controller.alertState);
            return;
        }
        
        //alert if attack duration elapsed
        if((attackDuration-=Time.deltaTime)<0)
        {
            controller.stateMachine.ChangeState(controller.alertState);
            return;
        } 
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(controller.damageDelay);
        controller.Attack();
    }

    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }
}