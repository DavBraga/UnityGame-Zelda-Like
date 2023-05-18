using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //TODO FIX IT
    [SerializeField] float swingPower = 50f;
    private void OnTriggerEnter(Collider other) {
      //  if(other.gameObject.GetComponent<BombScript>())
      //  {
          // Rigidbody bombRB = other.gameObject.GetComponent<Rigidbody>();
          // bombRB.AddForce(transform.forward*swingPower, ForceMode.VelocityChange);
      //  }
    }
}
