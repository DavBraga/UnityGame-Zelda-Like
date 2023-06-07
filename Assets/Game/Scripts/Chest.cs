using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    InteractiveObject interactionInterface;

    private void Awake() {
        interactionInterface = GetComponent<InteractiveObject>();
    }

    private void OnEnable() {
        interactionInterface.onInteraction +=ChestInteraction;
    }

    private void OnDisable() {   
        interactionInterface.onInteraction-=ChestInteraction;
    }

    public void ChestInteraction()
    {
        Debug.Log("Chest Interacted");
    }

}
