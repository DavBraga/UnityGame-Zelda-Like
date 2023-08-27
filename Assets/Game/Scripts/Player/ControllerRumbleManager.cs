using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerRumbleManager : MonoBehaviour
{
    Gamepad gamepad;
    Coroutine rumbleRoutine;

    public void RumblePulse(float lowFrequency,float highFrequency,float duration)
    {
        if(Application.isMobilePlatform&&Gamepad.all.Count<1)Handheld.Vibrate();
        if(Gamepad.all.Count<1) return;
        gamepad = Gamepad.current;
        gamepad?.SetMotorSpeeds(lowFrequency,highFrequency);
        if(rumbleRoutine!=null) StopCoroutine(rumbleRoutine);
        rumbleRoutine = StartCoroutine(StopRumbpleAfterAwhile(duration));
        
        
      
    }

    IEnumerator StopRumbpleAfterAwhile(float duration)
    {
        float rumbleStopTime = Time.time+duration;
        yield return new WaitUntil(()=> Time.time>rumbleStopTime);
        gamepad.SetMotorSpeeds(0,0);
    }
}
