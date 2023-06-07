using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
    
    [SerializeField] InteractionEvent interaction;
    [SerializeField] InteractionWidget interactionPlate;
    public UnityAction onInteraction;

    float customInteractionRadius = -1;
    bool inRange = false;
    bool canInteract = true;
    public Vector3 GetPosition()
    {
         return transform.position;
    }
    private void Start() {
        //interactionPlate.gameObject.SetActive(false);
    }

    public void Interact()
    {
        onInteraction?.Invoke();
        SetFlag(false);
        canInteract=false;
        interaction?.UnregisterListerner(this);
    }

    public void SetFlag(bool flag = true)
    {
        if(inRange==flag) return;

        if(flag)
        {
            interactionPlate.Show();
        }
            
        else interactionPlate.Hide();
        
        inRange = flag;
        Debug.Log("object is flagged: "+ inRange);
    }

    private void Awake() {
        //refatorar
        if(canInteract)
        interaction?.RegisterListerner(this);
    }
    private void OnEnable() 
    {
        if(canInteract)
        interaction?.RegisterListerner(this);
    }
    private void OnDisable() {

        interaction?.UnregisterListerner(this);
    }

    public float GetCustomRadius()
    {
        return customInteractionRadius;
    }
}
