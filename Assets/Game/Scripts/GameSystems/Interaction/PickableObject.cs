using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableObject : MonoBehaviour
{
    [SerializeField] string playerTag ="Player";
    [SerializeField] InventoryComunication inventoryChannel;
    [SerializeField] GameObject pickUpPopUp;
    [SerializeField] int spawnUpwardsPushPower=10;
    float spawnTime;
    bool isPickable = false;

    Rigidbody myrigidbody;
    public UnityAction<GameObject> onIntemUsed;
    [SerializeField] ItemSO item;
    [SerializeField] float pickableDelay = 1f;
    private void Awake() {
        myrigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        myrigidbody.AddForce(Vector3.up*spawnUpwardsPushPower, ForceMode.Impulse);
        spawnTime = Time.time;
    }
    private void OnTriggerEnter(Collider other) {
        if(Time.time<spawnTime+pickableDelay) return;
         if(other.gameObject.CompareTag(playerTag))
        {
           if(!inventoryChannel.AddItem(item))
           {
               //todo use
           }
           else
           {
                Destroy(this.gameObject);
                Instantiate(pickUpPopUp,transform.position, Quaternion.identity);
           }   
        }
        
    }
}
