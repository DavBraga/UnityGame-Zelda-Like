using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] InteractionEvent interaction;

    float distance;

    bool inRange = false;

    public void SetFlag(bool flag= true)
    {
        if(inRange) return;
        inRange = flag;
        Debug.Log("Chest is flagged");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Interact()
    {
        Debug.Log("Chest Interaction");
    }

    private void Awake() {
        //refatorar
        interaction?.RegisterListerner(this);
    }
    private void OnEnable() 
    {
         interaction?.RegisterListerner(this);
    }
    private void OnDisable() {

        interaction?.UnregisterListerner(this);
    }
}
