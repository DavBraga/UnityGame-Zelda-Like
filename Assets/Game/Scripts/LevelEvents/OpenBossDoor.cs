using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpenBossDoor : MonoBehaviour
{

   [SerializeField] Animator bossDoorAnimator;
    ConditionalAction conditionScript;
    private void Awake() {
        conditionScript = GetComponent<ConditionalAction>();
    }
    private void OnEnable() {
        if(conditionScript)
        conditionScript.onConditionsMet+= Open;
        
    }
    private void OnDisable() {
         if(conditionScript)
        conditionScript.onConditionsMet-= Open;
    }

    public void Open()
    {
        bossDoorAnimator.SetTrigger("tOpenDoor");
    }
}
