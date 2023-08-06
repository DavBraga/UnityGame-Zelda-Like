using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarHandler : MonoBehaviour
{
    [SerializeField] Slider refferedBar;
    // Start is called before the first frame update

    private void Awake() {
        if(!refferedBar) refferedBar = GetComponent<Slider>();
    }

    public bool InitializeValues(int startValue, int maxValue, int minValue = 0)
    {
        if(!refferedBar) return false;
        UpdateMinValue(minValue);
        UpdateMaxValue(maxValue);
        UpdateBarValue(startValue);
        
        return true;
    }

    public void UpdateBarValue(int value)
    {
        refferedBar.value=value;
    }
    
    public void UpdateBarValue(float value)
    {
        refferedBar.value=value;
    }
    public void UpdateMaxValue(int value)
    {
        refferedBar.maxValue = value;
    }
    public void UpdateMinValue(int value)
    {
        refferedBar.minValue = value;
    }
}
