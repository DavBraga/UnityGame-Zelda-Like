using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] GameObject pickUpPopUp;
    [SerializeField] string playerTag ="Player";
    [SerializeField] float pickableDelay = 1f;
    [SerializeField] int spawnUpwardsPushPower=10;

    Rigidbody myrigidbody;
    float spawnTime;

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


            Destroy(this.gameObject);
            Instantiate(pickUpPopUp,transform.position, Quaternion.identity);

        
    }
    }
}
