using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerinputs : MonoBehaviour
{
    [SerializeField]PlayerController player;
   [SerializeField] bool virtualInput = false;
    private void Start() {
        if(Application.isMobilePlatform||Application.platform==RuntimePlatform.WebGLPlayer)
        GameManager.Instance.onGAmeGoesPlayMode+=()=>{
            virtualInput = PlayerPrefs.GetInt("usingVirtualInput")>0;
            };
    }

    public void SetVirtualInput( bool virtualInputOn)
    {
        virtualInput = virtualInputOn;
    }

    public void SetPlayer(PlayerController newPlayer)
    {
        if(virtualInput) return;
        player = newPlayer;
    }
    public void SetMovment(InputAction.CallbackContext value)
    {
        if(virtualInput) return;
         player.SetMovmentVector(value.ReadValue<Vector2>());
    }
     public void SetUpJump(InputAction.CallbackContext value)
    {   if(virtualInput) return;
        player.TryJump(value.performed);
    }
    public void SetAttack(InputAction.CallbackContext value)
    {
        if(virtualInput) return;
        player.TryAttack(value.performed);
        player.attacking = !value.canceled;
    }
     public void SetBlock(InputAction.CallbackContext value)
    {
        if(virtualInput) return;
        player.TryBlock(value.performed);
        player.defending = !value.canceled;
    }
    public void SetUseTool(InputAction.CallbackContext value)
    {   
        if(virtualInput) return;
        if(value.performed)player.TryUseTool();     
    }
    public void SetUsePotion(InputAction.CallbackContext value)
    { 
        if(virtualInput) return;
        if(value.performed)player.TryUsePotion();
    }
    public void SetUseInteraction(InputAction.CallbackContext value)
    {
        if(virtualInput) return;
        if(value.performed)player.TryToInteract();
    }
    public void SetPauseGame(InputAction.CallbackContext value)
    {    
        if(virtualInput) return;
        if(value.performed)
            GameManager.Instance.TogglePause();
    }
     public void SetMap(InputAction.CallbackContext value)
    {
        if(virtualInput) return;
        player.TryEnlargeMap(value.performed);
    }
    
}
