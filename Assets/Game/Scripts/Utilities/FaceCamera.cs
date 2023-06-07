using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera cameraRef;
    // Start is called before the first frame update
    void Start()
    {
        if(!cameraRef) cameraRef = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cameraRef.transform.rotation;
    }
}
