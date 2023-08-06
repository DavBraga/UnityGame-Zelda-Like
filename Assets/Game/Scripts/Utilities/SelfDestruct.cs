using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float destructionDelayt = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destructionDelayt);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
