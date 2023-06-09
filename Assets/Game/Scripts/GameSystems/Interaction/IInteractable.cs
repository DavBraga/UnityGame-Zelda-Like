using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void Interact();
    public void SetCloseness(bool isColse=true);

    public void FlagForInteraction(bool canInteract= true);

    public bool CanInteract();

    public Vector3 GetPosition();
    public float GetCustomRadius();
}
