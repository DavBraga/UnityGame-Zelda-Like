using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalAction : MonoBehaviour
{
    [SerializeField] List<bool> switchs = new List<bool>();
    public UnityAction onConditionsMet;
    public UnityEvent onCoditionsMetEvent;
    public void TurnSwitch(int switchIndex)
    {
        if(switchIndex<0 || switchIndex> switchs.Count-1 ) return ;

        switchs[switchIndex] = true;
        if(CheckConditons())
        {
            onConditionsMet?.Invoke();
            onCoditionsMetEvent?.Invoke();
        } 
   
    }

    public bool CheckConditons()
    {
        foreach(bool condition in switchs)
        {
            if(!condition) return false;
        }
        return true;
    }
}
