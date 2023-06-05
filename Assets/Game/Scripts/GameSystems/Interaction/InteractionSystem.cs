using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] float distanceCheckInterval = .3f;
    [SerializeField] InteractionEvent interactionEvent;

    Transform playerTransform;
    float distanceCheckCooldown;
    [SerializeField] float interactionRadius;
    public UnityAction checkDistance;

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
        Interact();
        if((distanceCheckInterval-=Time.deltaTime)>0) return;
        
        distanceCheckCooldown = distanceCheckInterval;
        CheckListernsInRadius();
    }
    public void TrySetTheInteraction(IInteractable interactable, float distance)
    {
        if(interactingOBject.distance>0 && interactingOBject.distance<distance) return;

        interactingOBject.currentObject = interactable;
        interactingOBject.distance = distance; 
        
    }

    public void Interact()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //only if valid distance
            if(interactingOBject.distance<0)
            {
                Debug.Log("None in range to interact");
                return;
            }
            
            interactingOBject.currentObject.Interact();
        } 

    }

    public bool InInteractionRadius(IInteractable interactable,Vector3 interactablePosition,float Radius)
    {
        if(!playerTransform)
        {
            if(!GameManager.Instance) return false;
            playerTransform = GameManager.Instance.GetPlayer().transform;
        }


        float distance=(Vector3.Distance(interactablePosition, playerTransform.position));
        if(distance>interactionRadius) return false;

        interactable.SetFlag();
        TrySetTheInteraction(interactable, distance);
        return true;
    }

    public void CheckListernsInRadius()
    {
       List<IInteractable> interactablesList = interactionEvent.GetInteractionList();
       if(interactablesList.Count<1) return;
       bool gotAny = false; 
       for(int i = interactablesList.Count-1;i>=0;i--)
       {   
            
            IInteractable interactable=interactablesList[i];
            gotAny = InInteractionRadius(interactable, interactable.GetPosition(), interactionRadius); 
       }
       //if none found in radius set Invalid distance
       if(!gotAny) interactingOBject.distance = -1;
    }
}
