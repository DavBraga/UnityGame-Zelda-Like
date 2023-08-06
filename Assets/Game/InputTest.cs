using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
     public void SetMovment(InputAction.CallbackContext value)
    {
        //inputMovmentVector = value.ReadValue<Vector2>();
        Debug.Log("brazil!");
    }

    public void SetUpJump(InputAction.CallbackContext value)
    {
      Debug.Log("pulou");
        //Jump();
    }

    public void Jump()
    {
        Debug.Log("jump");
    }

    public void Move()
    {
        Debug.Log("move");
    }
}
