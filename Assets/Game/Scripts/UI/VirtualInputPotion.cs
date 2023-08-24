using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualInputPotion : MonoBehaviour
{
   UIVirtualButton potionButton;
    private void Awake() {
        potionButton = GetComponent<UIVirtualButton>();
        potionButton.interactable = PlayerPrefs.GetInt("usingVirtualInput")>0&&(Application.isMobilePlatform||Application.platform==RuntimePlatform.WebGLPlayer);
        
    }
    public void EnableDisableButtonInteraction(bool on)
    {
        potionButton.interactable = on;
    }
}
