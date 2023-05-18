
using UnityEngine;

public abstract class State
{
    public readonly string stateName;
    protected State(string name)
    {
        stateName= name;
    }
    public virtual void OnStateEnter(){}
    public virtual void OnStateExit(){}
    public virtual void OnStateUpdate(){}
    public virtual void OnStateFixedUpdate(){}
    public virtual void OnStateLateUpdate(){}

}
