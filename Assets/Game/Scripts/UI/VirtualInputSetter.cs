using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class VirtualInputSetter : MonoBehaviour
{
    public UnityAction<bool> onVirtualInputToggles;
    [SerializeField] bool preventPCfromUsingVirtualInput = true;
    Toggle mobileInputToggler;

    private void Awake() {
        mobileInputToggler = GetComponent<Toggle>();
    }
    void Start()
    {
        if(!preventPCfromUsingVirtualInput) return;
        if(Application.platform!= RuntimePlatform.WebGLPlayer)
        {
            TurnVirtualInput(false);
            gameObject.SetActive(false);
        }
        if(Gamepad.all.Count>0)
        {
            TurnVirtualInput(false);
            gameObject.SetActive(false);
            return;
        }

        if(Application.isMobilePlatform)
        {
            gameObject.SetActive(false);
            TurnVirtualInput(true);
            return;
        }
      
        
    }
    public void TurnVirtualInput(bool on)
    {
        if(on)
        PlayerPrefs.SetInt("usingVirtualInput",1);
        else PlayerPrefs.SetInt("usingVirtualInput",0);
        onVirtualInputToggles?.Invoke(on);
    }


    private void OnEnable() 
    {
       if( PlayerPrefs.GetInt("usingVirtualInput")>0)
       mobileInputToggler.isOn = true;
       else mobileInputToggler.isOn = false;
    }
}
