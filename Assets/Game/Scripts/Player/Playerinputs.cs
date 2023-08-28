using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Playerinputs : MonoBehaviour
{
    [SerializeField]float damageRumbleDuration = .33f;
    [SerializeField] bool vibration= true;
    public UnityAction<int> onVibrationChanges;
    Gamepad gamepad;
    Coroutine rumbleRoutine;
    [SerializeField]PlayerAvatar player;
   [SerializeField] bool virtualInput = false;

   private void Awake() {

    if(!PlayerPrefs.HasKey("vibration")&&!Application.isMobilePlatform)PlayerPrefs.SetInt("vibration",1);
    if(PlayerPrefs.GetInt("vibration")>0) vibration = true;
    else vibration= false;
   }
    private void Start() {
        if(Application.isMobilePlatform||Application.platform==RuntimePlatform.WebGLPlayer)
        GameManager.Instance.onGAmeGoesPlayMode+=()=>{
            virtualInput = PlayerPrefs.GetInt("usingVirtualInput")>0;
            };
    }

    private void OnEnable() {
        player.GetComponent<PlayerDeath>().onTakeDamageAction+= DamageRumble;
    }
    private void OnDisable() {
        player.GetComponent<PlayerDeath>().onTakeDamageAction-= DamageRumble;
    }

    public void SetVibration(bool value)
    {
        vibration = value;
    }

    public void SetVirtualInput( bool virtualInputOn)
    {
        virtualInput = virtualInputOn;
    }

    public void SetPlayer(PlayerAvatar newPlayer)
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

    public void RumblePulse(float lowFrequency,float highFrequency,float duration)
    {
        if(!vibration) return;
        if(Application.isMobilePlatform&&Gamepad.all.Count<1)
        {
            Handheld.Vibrate();
            return;
        }

        if(Gamepad.all.Count<1) return;
        gamepad = Gamepad.current;
        
        gamepad?.SetMotorSpeeds(lowFrequency,highFrequency);
        if(rumbleRoutine!=null) StopCoroutine(rumbleRoutine);
        rumbleRoutine = StartCoroutine(StopRumbpleAfterAwhile(duration));  
    }

    public void DamageRumble()
    {
        RumblePulse(.25f,.85f,damageRumbleDuration*.65f);
    }

    IEnumerator StopRumbpleAfterAwhile(float duration)
    {
        float rumbleStopTime = Time.time+duration;
        yield return new WaitUntil(()=> Time.time>rumbleStopTime);
        gamepad.SetMotorSpeeds(0,0);
    }
    
}
