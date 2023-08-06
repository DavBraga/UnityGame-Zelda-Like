using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate bool boolConditionDelegate();
public class MoodState : State
{
    BossControler controller;
    boolConditionDelegate leavingCondition;

    public UnityAction onEnterStateEffect;
    public UnityAction onExitStateEffect;

    State nextMoodState;
    
    public MoodState(BossControler controller,string name = "moodState", boolConditionDelegate condition=null, MoodState nextMoodState=null ) : base(name)
    {
        this.controller = controller;
        if(condition!=null)
            leavingCondition = condition;
        else 
            leavingCondition = ()=>{return false;};
        
        this.nextMoodState = nextMoodState;
    }

    public void SetUpOnEnterEffect(UnityAction action)
    {
        onEnterStateEffect = null;
        onEnterStateEffect += action;
    }
     public void SetUpOnExitEffect(UnityAction action)
    {
        onExitStateEffect = null;
        onExitStateEffect += action;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if(leavingCondition())
        {
            controller.moodStateMachine.ChangeState(nextMoodState);
        }
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        onEnterStateEffect?.Invoke();
        Debug.Log("Enter "+stateName+" state.");
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
        onExitStateEffect?.Invoke();
        Debug.Log("Leaves"+stateName+" state");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
