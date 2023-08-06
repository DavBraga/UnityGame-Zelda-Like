using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicBoss : MonoBehaviour
{
    Animator controller;

    private void Awake() {
        controller= GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LookAround()
    {
        controller.SetTrigger("tLookAround");
    }
}
