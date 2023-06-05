using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void Interact();
    public void SetFlag(bool flag=true);

    public Vector3 GetPosition();
}
