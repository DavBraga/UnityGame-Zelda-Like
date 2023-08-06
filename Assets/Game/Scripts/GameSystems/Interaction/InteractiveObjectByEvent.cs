using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObjectByEvent : MonoBehaviour, IInteractable
{
    [SerializeField] InteractionEvent interaction;
    [SerializeField]bool canInteract = true;
    [SerializeField] float customInteractionRadius = -1;
    InteractionWidget interactionPlateScript;
    [SerializeField] GameObject interactionPlateObject;

    public UnityEvent onInteraction, onClose, onLeave;
    bool inRange = false;

    private void Awake() {
        interaction?.RegisterListerner(this);
        GameObject interactionPlate= Instantiate(interactionPlateObject,gameObject.transform);
        interactionPlateScript = interactionPlate.GetComponent<InteractionWidget>();
    }
    private void OnEnable() 
    {
        interaction?.RegisterListerner(this);
    }
    private void OnDisable() {

        interaction?.UnregisterListerner(this);
    }


    public bool CanInteract()
    {
        return canInteract;
    }

    public void FlagForInteraction(bool interactionFlag)
    {
        if(interactionFlag)   
            interactionPlateScript.Show();
        else
            interactionPlateScript.Hide();
    }

    public float GetCustomRadius()
    {
        return customInteractionRadius;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public void Interact(GameObject interactor = null)
    {
        if(!canInteract) return;
        onInteraction?.Invoke();
        SetCloseness(false);
        canInteract=false;
        interaction?.UnregisterListerner(this);
        interactionPlateScript.Hide();
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
        } 
        
        inRange = isClose;
    }
}
