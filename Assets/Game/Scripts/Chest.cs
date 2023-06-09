using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    InteractiveObject interactionInterface;
    [SerializeField] Animator ChestAnimator;
    [SerializeField] ItemSO treasureItem;
    [SerializeField] InventoryComunication inventoryChannel;
    [SerializeField] Transform contentHolder;

    private void Awake() {
        interactionInterface = GetComponent<InteractiveObject>();
    }

    private void Start() {
        Instantiate(treasureItem.GetItemPrefab(),transform.position,Quaternion.identity, contentHolder);
    }
    private void OnEnable() {
        interactionInterface.onInteraction +=ChestInteraction;
        interactionInterface.onClose += OnGetClose;
        interactionInterface.onLeave+= OnLeaveRadius;
    }

    private void OnDisable() {   
        interactionInterface.onInteraction-=ChestInteraction;
        interactionInterface.onClose -= OnGetClose;
        interactionInterface.onLeave -= OnLeaveRadius;
    }

    public void ChestInteraction()
    {
        Debug.Log("Chest Interacted");
        ChestAnimator.SetTrigger("tOpen");
        if(inventoryChannel)
            inventoryChannel.AddItem(treasureItem);
    }
    public void OnGetClose()
    {
        ChestAnimator.SetBool("bClose", true);
        
    }
    public void OnLeaveRadius()
    {
        ChestAnimator.SetBool("bClose", false);
    }

}
