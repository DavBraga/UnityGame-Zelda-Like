using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionEvent", menuName = "Zelda Like/InteractionEvent", order = 0)]
public class InteractionEvent : ScriptableObject {

    private List<IInteractable> listeners= new List<IInteractable>();

    public bool RegisterListerner(IInteractable listerner)
    {
        if(listeners.Contains(listerner)) return false;
        listeners.Add(listerner);
        return true;

    }
    public bool UnregisterListerner(IInteractable listerner)
    {
         if(!listeners.Contains(listerner)) return false;
        listeners.Remove(listerner);
        return true;
    }
    public void PurgeListeners()
    {
        listeners.Clear();
    }

    public List<IInteractable> GetInteractionList()
    {
        return listeners;

    }
}
