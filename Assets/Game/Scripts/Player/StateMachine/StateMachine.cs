using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public State currentState{get; private set;}
    public State LastState{get; private set;}

    public void ChangeState(State newState)
    {
        currentState?.OnStateExit();
        if(LastState!=currentState)LastState = currentState;
        currentState = newState;
        currentState?.OnStateEnter();
    }

    public void BackToLastState()
    {
        currentState?.OnStateExit();
        currentState = LastState;
        currentState?.OnStateEnter();
    }

    public string GetCurrentStateName()
    {
        return currentState?.stateName;
    }
    // Update is called once per frame
    public void Update()
    {
        currentState?.OnStateUpdate();
    }
    public void FixedUpdate() {
        currentState?.OnStateFixedUpdate();
    }
    public void LateUpdate() {
        currentState?.OnStateLateUpdate();
    }
}
