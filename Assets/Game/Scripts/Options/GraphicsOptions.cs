using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class GraphicsOptions : MonoBehaviour
{
    [SerializeField] UniversalRenderPipelineAsset piplelineAsset; 
    [SerializeField]RenderPipelineAsset currentAsset;
    [SerializeField] bool applyExpensiveChanges = false;
    
    public void ChangeResolutionScale(float value)
    {
        piplelineAsset.renderScale = value*0.01f;
       // piplelineAsset = currentAsset as UniversalRenderPipelineAsset;
    }

    private void OnEnable() {
        currentAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
        piplelineAsset = currentAsset as UniversalRenderPipelineAsset;
    }

    public void ChangeQuality(int index)
    {
        if(index ==QualitySettings.GetQualityLevel()) return;
        QualitySettings.SetQualityLevel(index,applyExpensiveChanges);
        currentAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
        piplelineAsset = currentAsset as UniversalRenderPipelineAsset;
    }
}

