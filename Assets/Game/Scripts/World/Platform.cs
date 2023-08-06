using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(playerTag))
        {
            other.GetComponent<MoveWithPlatform>().SetPlatform(transform);
        }
            
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag(playerTag))
            other.GetComponent<MoveWithPlatform>().LeavePlatform();
    }
}
