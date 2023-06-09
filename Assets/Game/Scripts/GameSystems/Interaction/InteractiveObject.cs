using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
    
    [SerializeField] InteractionEvent interaction;
    [SerializeField] InteractionWidget interactionPlate;
    
    public UnityAction onInteraction, onClose, onLeave;

    float customInteractionRadius = -1;
    bool inRange = false;
    [SerializeField]bool canInteract = true;
    public Vector3 GetPosition()
    {
         return transform.position;
    }
    private void Start() {
        //interactionPlate.gameObject.SetActive(false);
    }

    public void Interact()
    {
        if(!canInteract) return;
        onInteraction?.Invoke();
        SetCloseness(false);
        canInteract=false;
        interaction?.UnregisterListerner(this);
         
    }

    public void SetCloseness(bool isClose = true)
    {
        if(inRange==isClose) return;

        if(isClose)
        {
            onClose?.Invoke();
        }
        else
        {
            onLeave?.Invoke();
            interactionPlate.Hide();
        } 
        
        inRange = isClose;
        Debug.Log("object is flagged: "+ inRange);
    }

    private void Awake() {
        interaction?.RegisterListerner(this);
    }
    private void OnEnable() 
    {
        interaction?.RegisterListerner(this);
    }
    private void OnDisable() {

        interaction?.UnregisterListerner(this);
    }

    public float GetCustomRadius()
    {
        return customInteractionRadius;
    }

    public void TurnInteraction(bool turn=true)
    {
        canInteract= turn;
    }

    public void FlagForInteraction(bool interactionFlag)
    {
        if(interactionFlag)   
            interactionPlate.Show();
        else
            interactionPlate.Hide();
    }
    public bool CanInteract()
    {
        return canInteract;
    }
}
