using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSelector : MonoBehaviour
{
   [SerializeField] GameObject gamepadTutorial;
    [SerializeField]GameObject keyboardTutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireTutorial()
    {
        if(Gamepad.all.Count>0) gamepadTutorial.gameObject.SetActive(true);
        else keyboardTutorial.gameObject.SetActive(true);
        
    }
}
