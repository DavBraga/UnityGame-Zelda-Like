using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityDropdown : MonoBehaviour
{
    TMP_Dropdown dropdown;

    private void Awake() {
        dropdown = GetComponent<TMP_Dropdown>();
        //LoadCurrentQuality();
    }
    private void OnEnable() 
    {
        
        if(dropdown)LoadCurrentQuality();
    }

    public void LoadCurrentQuality()
    {
        if(dropdown.value!=QualitySettings.GetQualityLevel())
        dropdown.value=QualitySettings.GetQualityLevel();
    }
}
