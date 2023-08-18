using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScaleText : MonoBehaviour
{
    [SerializeField] string preText;
    [SerializeField]TextMeshProUGUI text;
    [SerializeField] string postText;
    public void UpdateScaleText(float sliderValue)
    {
        string textvalue=sliderValue.ToString();
        text.text = preText+textvalue+postText;

    }
}
