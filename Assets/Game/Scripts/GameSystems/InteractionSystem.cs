using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] float distanceCheckInterval = .3f;

    Transform playerTransform;
    float distanceCheckCooldown;
    float interactionRadius;
    public UnityAction checkDistance;

    public List<IInteractable> interactables = new List<IInteractable>();

    interactableStruct interactingOBject;

    public struct interactableStruct{
        public IInteractable currentObject;
        public float distance;
    }



    void Start()
    {
        distanceCheckCooldown = distanceCheckInterval;
        interactingOBject.distance = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if((distanceCheckInterval-=Time.deltaTime)>0) return;
        
        distanceCheckCooldown = distanceCheckInterval;
        //checkDistance;
        foreach(IInteractable interactable in interactables)
        {
            InInteractionRadius(interactable,interactable.GetPosition(), interactionRadius);
        }
    }
    public void TrySetTheInteraction(IInteractable interactable, float distance)
    {
        if(interactingOBject.distance>0 && interactingOBject.distance<distance) return;

        interactingOBject.currentObject = interactable;
        interactingOBject.distance = distance; 
        
    }

    public void Interact()
    {
        if(interactingOBject.distance>-1)
        {
           if(Input.GetKeyDown(KeyCode.E))
           {
                interactingOBject.currentObject.Interact();
           } 
        }
    }

    public void InInteractionRadius(IInteractable interactable,Vector3 interactablePosition,float Radius)
    {
        float distance=(Vector3.Distance(interactablePosition, playerTransform.position));
        if(distance>interactionRadius) return;

        interactable.SetFlag();
        TrySetTheInteraction(interactable, distance);
        


    }
}
