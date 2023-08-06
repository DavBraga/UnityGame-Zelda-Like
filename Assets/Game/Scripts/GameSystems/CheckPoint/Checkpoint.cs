using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent restoreEvent;
    [SerializeField] InventoryComunication inventoryChannel;
    [SerializeField] ItemSO potions;
    [SerializeField]Transform checkpointPosition;

    [SerializeField] string checkPointName="";

    Collider mycollider;
    private void Awake() {
        mycollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other) {
        
        if(other.CompareTag("Player"))
        {
            //mycollider.enabled = false;
           other.GetComponent<CheckPointManager>().SaveCheckPoint(checkpointPosition,inventoryChannel.GetItemCount(potions),PlayEvents,checkPointName);
        }
    }

    public void PlayEvents()
    {
        Debug.Log("Calls");
        restoreEvent?.Invoke();
    }
}
