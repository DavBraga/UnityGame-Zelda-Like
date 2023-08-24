using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsOptions : MonoBehaviour
{
    [SerializeField] UniversalRenderPipelineAsset piplelineAsset; 
    [SerializeField]RenderPipelineAsset currentAsset;
    [SerializeField] bool applyExpensiveChanges = false;

    private void Awake() {
        if(!PlayerPrefs.HasKey("graphicQuality")||!PlayerPrefs.HasKey("resolutionScale")) return;

        ChangeQuality(PlayerPrefs.GetInt("graphicQuality"));
        ChangeResolutionScale(PlayerPrefs.GetFloat("resolutionScale"));
    }
    
    public void ChangeResolutionScale(float value)
    {
        piplelineAsset.renderScale = value*0.01f;
        PlayerPrefs.SetFloat("resolutionScale", value);
    }

    private void OnEnable() {
        currentAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
        piplelineAsset = currentAsset as UniversalRenderPipelineAsset;
    }

    public void ChangeQuality(int index)
    {
        if(index ==QualitySettings.GetQualityLevel()) return;
        QualitySettings.SetQualityLevel(index,applyExpensiveChanges);
        PlayerPrefs.SetInt("graphicQuality", index);
        currentAsset = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel());
        piplelineAsset = currentAsset as UniversalRenderPipelineAsset;
    }
}

