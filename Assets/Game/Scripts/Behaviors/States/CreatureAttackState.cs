using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackState : State
{
    CreatureController controller;
    float attackDuration = 0;
    IEnumerator attackCoroutine;

    State fallBackState;
    public CreatureAttackState(CreatureController controller) : base("CreatureAttackState")
    {
        this.controller =controller;
    }
    public void SetUpState(State fallBackState)
    {
        this.fallBackState = fallBackState;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        attackDuration = controller.baseAttack.AttackDuration;
        controller.StartCoroutine(attackCoroutine=StartAttack());
        
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        controller.StopCoroutine(attackCoroutine);
    }
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
         //fall back after duration elapsed
        if((attackDuration-=Time.deltaTime)<0)
        {
            controller.StopCoroutine(attackCoroutine);
            controller.stateMachine.ChangeState(fallBackState);
            return;
        } 
    }
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }

    IEnumerator StartAttack()
    {
        controller.myAnimator.SetTrigger(controller.baseAttack.animationTag);
        yield return new WaitForSeconds(controller.baseAttack.DamageDelay);

        // alert if its not in attackrange
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position;
        
        bool attackRangeCheck = 
            CreatureHelper.IstargetInRange(controller.baseAttack.AttackRadius,
            controller.transform.position,targetPos);

        if(!attackRangeCheck)
        {
            controller.stateMachine.ChangeState(fallBackState);
            controller.RangedAttack();
        }
        else
            controller.Attack();
    }
}
