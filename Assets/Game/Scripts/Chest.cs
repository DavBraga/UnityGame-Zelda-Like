using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public InteractionSystem interactionSystem;
    public float interactionRadius = 3;

    float distance;

    bool inRange = false;

    public void SetFlag(bool flag= true)
    {
        inRange = flag;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        Debug.Log("interacts...");
        interactionSystem.interactables.Remove(this);
    }

    private void Awake() {
        //refatorar
        interactionSystem.interactables.Add(this);
    }
}
