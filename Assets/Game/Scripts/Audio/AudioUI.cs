using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioUI : MonoBehaviour
{
    [SerializeField] MixerController mixerController;

    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider SFXVolume;
    [SerializeField] Slider voiceVolume;

    public void LoadSliders()
    {
        mixerController.GetMixer().GetFloat("MasterVolume", out float volume);
        masterVolume.value = volume;
        
        mixerController.GetMixer().GetFloat("MusicVolume",out volume);
        musicVolume.value = volume;
        
        mixerController.GetMixer().GetFloat("VoicesVolume", out volume);
        voiceVolume.value = volume;
        
        mixerController.GetMixer().GetFloat("SFXVolume", out volume);
        SFXVolume.value = volume;

    }
}
