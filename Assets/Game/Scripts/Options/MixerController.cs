using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField]AudioMixer mixer;

     public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("MasterVolume",Mathf.Log10(sliderValue)*20);
    }
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume",Mathf.Log10(sliderValue)*20);
    }
    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SFXVolume",Mathf.Log10(sliderValue)*20);
    }
     public void SetVoiceVolume(float sliderValue)
    {
        mixer.SetFloat("VoicesVolume",Mathf.Log10(sliderValue)*20);
    }

    public AudioMixer GetMixer()
    {
        return mixer;
    }
   
}
