using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControler : CreatureController
{
    [Range(0f, 1f)]
    [SerializeField] float healthThreshold= .4f;

    public override void SetUpStates()
    {
        base.SetUpStates();
        stateMachine.ChangeState(chaseState);
        chaseState.SetUpState(creatureAttackState, creatureRoamingState, creatureAttackState);
    }
    public override void Update() {

        base.Update();
        //Debug.DrawRay(transform.position, transform.forward*3);
        
    }
}
