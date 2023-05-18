using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] float[] possibleRotations;
    // Start is called before the first frame update

    private void OnEnable() {
        Vector3 newRot = new Vector3(transform.rotation.x, possibleRotations[Random.Range(0, possibleRotations.Length)], transform.rotation.z);
        transform.Rotate(newRot);
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}
