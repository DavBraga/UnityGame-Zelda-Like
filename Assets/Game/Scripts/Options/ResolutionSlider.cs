using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class ResolutionSlider : MonoBehaviour
{
    [SerializeField] Slider resolutionSlider;
    bool gotResolutionSlider;

    private void Awake() {
        resolutionSlider = GetComponent<Slider>();
        gotResolutionSlider = true;
        LoadCurrentResolution();

    }

    private void OnEnable() {
        if(gotResolutionSlider)
        LoadCurrentResolution();
        else
        StartCoroutine(WaitForSlider());
    }
    IEnumerator WaitForSlider()
    {
        yield return new WaitUntil(()=> resolutionSlider);
        LoadCurrentResolution();

    }
    public void LoadCurrentResolution()
    {
        UniversalRenderPipelineAsset asset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;
        if(gotResolutionSlider)
        resolutionSlider.value =  asset.renderScale*100;
        else
        StartCoroutine(WaitForSlider());
    }
}
