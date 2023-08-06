using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
     InteractiveObject interactionInterface;
    [SerializeField] Animator DoorAnimator;
    [SerializeField] ItemSO key;
    [SerializeField] InventoryComunication inventoryChannel;
    // Start is called before the first frame update

    private void Awake() {
        interactionInterface = GetComponent<InteractiveObject>();
    }

    private void OnEnable() {
        interactionInterface.onInteraction +=DoorInteraction;
        interactionInterface.onClose += OnGetClose;
        interactionInterface.onLeave+= OnLeaveRadius;
    }

    private void OnDisable() {   
        interactionInterface.onInteraction-=DoorInteraction;
        interactionInterface.onClose -= OnGetClose;
        interactionInterface.onLeave -= OnLeaveRadius;
    }

    public void DoorInteraction(GameObject interactor = null)
    {
        if(!inventoryChannel) return; 
        if(inventoryChannel.RemoveItem(key))
        {
            DoorAnimator.SetTrigger("tOpenDoor");
        }
    }
    public void OnGetClose()
    {
        Debug.Log("gets close to a door");
        interactionInterface.TurnInteraction(inventoryChannel.GetItemCount(key)>0);
        
    }
    public void OnLeaveRadius()
    {

    }

}
