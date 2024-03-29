using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    public UnityAction onANewInteractingObject;
    public UnityAction onClearInteractingObject;
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
        StartCoroutine(WaitForAPlayer());
    }

    IEnumerator WaitForAPlayer()
    {
        yield return new WaitUntil(()=>GameManager.IsManagerReady());
        yield return new WaitUntil(()=>GameManager.Instance.CheckForPlayer());
        GameManager.Instance.GetPlayer().GetComponent<PlayerAvatar>().onInteractHook+=Interact;
    }
    // Update is called once per frame
    void Update()
    {
        if((distanceCheckCooldown-=Time.deltaTime)>0) return;
        distanceCheckCooldown = distanceCheckInterval;
        if(interactingOBject.distance<0)onClearInteractingObject?.Invoke();
        CheckListernsInRadius();
    }
    public void TrySetTheInteraction(IInteractable interactable, float distance)
    {    
        if(interactingOBject.distance>0 && interactingOBject.distance<distance) return;
        
        if(interactable==interactingOBject.currentObject) return;

        SetInteractingObject(interactable, distance);
    }

    public void SetInteractingObject(IInteractable interactable, float distance)
    {
        Debug.Log("Setting new interactable");
        if(interactingOBject.distance>0)
        {
            //interactingOBject.currentObject.FlagForInteraction(false);
            ClearInteractingObject();

        }
            

        interactingOBject.currentObject = interactable;
        interactingOBject.distance = distance; 
        interactingOBject.currentObject.FlagForInteraction();
        onANewInteractingObject?.Invoke();
        
    }

    public void Interact()
    {
        CheckListernsInRadius();
        //only if valid distance
        if (interactingOBject.distance < 0)
        {
            Debug.Log("None in range to interact");
            return;
        }

        interactingOBject.currentObject.Interact(playerTransform.gameObject);
        ClearInteractingObject();
        CheckListernsInRadius();


    }

    private void ClearInteractingObject()
    {
        if(interactingOBject.distance <0) return;
        interactingOBject.distance = -1;
        interactingOBject.currentObject.FlagForInteraction(false);
        onClearInteractingObject?.Invoke();
    }

    public void CheckListernsInRadius()
    {   
        if(interactionEvent.GetInteractionList().Count<1) return;
        bool gotAny = false; 

        // try get player transform if none
        if(!playerTransform)
        {
            if(!GameManager.Instance) return;
            playerTransform = GameManager.Instance.GetPlayer().transform;
        }

        //compar
        for(int i = interactionEvent.GetInteractionList().Count-1;i>=0;i--)
        {   
            
            IInteractable interactable=interactionEvent.GetInteractionList()[i];
            float interactionRef;
            if((interactionRef = interactable.GetCustomRadius())<0)
                interactionRef = interactionRadius;

            
            float distance=(Vector3.Distance(interactable.GetPosition(), playerTransform.position));
            
            if(distance<interactionRef)
            {      
                interactable.SetCloseness();

                if(!interactable.CanInteract()) continue;
                TrySetTheInteraction(interactable,distance);
                gotAny = true;
            }
            else
            {
                 interactable.SetCloseness(false);
                 
            }   
        }
       //if none found in radius set Invalid distance
       if(!gotAny&&interactingOBject.distance>0)
       {
        interactingOBject.currentObject.FlagForInteraction(false);
        interactingOBject.currentObject=null;
        interactingOBject.distance = -1; 
       }
    }
}
