using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo rotations
public class MoveWithPlatform : MonoBehaviour
{
    bool onPlatform;

    public Transform platform;
    Vector3 platformLastPostion;

    public void LeavePlatform()
    {
        onPlatform = false;
    }

    public void SetPlatform(Transform _platform)
    {
        platform = _platform;
        platformLastPostion = platform.position;
        onPlatform = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(onPlatform)
        {
            transform.position +=  platform.position-platformLastPostion;
            platformLastPostion = platform.position;
       // transform.position += Vector3.forward*0.5f*Time.deltaTime;
       }
    }
}
