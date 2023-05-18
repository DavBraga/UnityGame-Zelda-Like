using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeedInDeegrees = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,rotationSpeedInDeegrees*Time.deltaTime,0);
        
    }
}
