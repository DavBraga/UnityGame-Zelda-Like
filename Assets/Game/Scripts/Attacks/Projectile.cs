using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed=1f;
    [SerializeField] float duration = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,duration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*Time.deltaTime*speed,Space.World);
        //transform.Translate()
        Debug.DrawRay(transform.position, transform.forward*3);
        
    }
}
