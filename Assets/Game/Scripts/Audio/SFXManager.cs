using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField]Vector2 pitchRange;

    [SerializeField]Vector2 volumeRange;
    [SerializeField] int audioRatio =3;
    [SerializeField] AudioClip[] clips;
    [SerializeField]bool playOnStart = false;
    int audioToPlay =0;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        RandomizeSound();
        audioSource.clip=clips[0];
    }
    private void Start() {
        //if(playOnStart) PlayAudio();
    }
    private void OnEnable() {
        if(playOnStart) audioSource.PlayOneShot(audioSource.clip);
        RandomizeSound();
    }
    public void PlayAudio()
    {
        if(audioToPlay>=clips.Length)
        {
            Debug.Log("no audio");
            RandomizeSound();
            return;
        } 
        audioSource.PlayOneShot(clips[audioToPlay]);
        RandomizeSound();
    }

    public void PlayAudio(AudioClip clip)
    {
        if(audioToPlay>=clips.Length)
        {
            Debug.Log("no audio");
            RandomizeSound();
            return;
        } 
        audioSource.PlayOneShot(clip);
        RandomizeSound();
    }

    public void PlayAudioOnRequest(AudioClip clip, float volume, float pitch)
    {
        audioSource.PlayOneShot(clip);
        audioSource.volume = volume;
        audioSource.pitch =pitch;
    }
    public void PlayAudioOnRequest(AudioClip clip, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
    public void PlayAudioOnRequest(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    private void RandomizeSound()
    {
        audioToPlay = Random.Range(0, clips.Length*audioRatio);
        if(audioToPlay>=clips.Length) return;
        audioSource.clip = clips[audioToPlay];
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);

    }
}
