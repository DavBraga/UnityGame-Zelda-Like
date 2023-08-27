using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationSetting : MonoBehaviour
{
    [SerializeField] Toggle vibrationToggle;

    private void Awake() {
        vibrationToggle = GetComponent<Toggle>();
        if(!PlayerPrefs.HasKey("vibration")&& !Application.isMobilePlatform)PlayerPrefs.SetInt("vibration",1);
    }
    private void OnEnable() {
        if(PlayerPrefs.GetInt("vibration")>0) vibrationToggle.isOn = true;
        else vibrationToggle.isOn = false;
        vibrationToggle.onValueChanged.AddListener(ToggleVibration);
    }
    private void OnDisable() {
        vibrationToggle.onValueChanged.RemoveListener(ToggleVibration);
    }
    public void ToggleVibration(bool value)
    {
        if(value)
        PlayerPrefs.SetFloat("vibration",1);
        else PlayerPrefs.SetFloat("vibration",0);
        PlayerPrefs.Save();
    }
    
}
