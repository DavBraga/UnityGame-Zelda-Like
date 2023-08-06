using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentTest : MonoBehaviour
{
    public bool moving;
    public string playerTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(playerTag))
        {
            other.GetComponent<MoveWithPlatform>().SetPlatform(transform);
            moving = true;
        }
            
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag(playerTag))
            other.GetComponent<MoveWithPlatform>().LeavePlatform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moving)
        transform.position = transform.position+ Vector3.forward*0.5f*Time.deltaTime;
        
    }
}
