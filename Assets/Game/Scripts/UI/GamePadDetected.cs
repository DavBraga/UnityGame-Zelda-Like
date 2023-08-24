using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePadDetected : MonoBehaviour
{
    [SerializeField]bool reverse = false;
    private void Start() {
            gameObject.SetActive(reverse!=(Gamepad.all.Count>0));
    }
    
}
