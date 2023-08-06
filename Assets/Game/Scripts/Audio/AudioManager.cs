using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] bool startWithAfadeIn=false;
    [SerializeField]float standardFadeDuration = 3f;
    [SerializeField] float startVolume = .8f;

    Coroutine fadeInProgress,transitionInProgress;

    private void Awake() {
        audioSource =GetComponent<AudioSource>();
    }

    private void Start() {
        if(startWithAfadeIn)
        {
            audioSource.volume = 0;
            FadeVolume(startVolume,standardFadeDuration);
        }
        
        
    }
    public void Play()
    {
        audioSource.Play();
    }

    public void SetMusic(AudioClip newClip)
    {
        audioSource.clip = newClip;
    }

    public void PlayAudio(AudioClip newClip)
    {
        SetMusic(newClip);
        audioSource.Play();
    }
    public void StandarTransition(AudioClip newclip)
    {
        if(transitionInProgress !=null) StopCoroutine(transitionInProgress);
        StartCoroutine(TransitionRoutine(newclip,standardFadeDuration));
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void FadeVolume(float desiredFinalVolume,float duration)
    {
        if(fadeInProgress!= null) StopCoroutine(fadeInProgress);
        fadeInProgress = StartCoroutine(FadeRoutine(desiredFinalVolume, duration));
    }
    

    public void StandardFadeVolume(float desiredFinalVolume)
    {
        if(fadeInProgress!= null) StopCoroutine(fadeInProgress);
        fadeInProgress = StartCoroutine(FadeRoutine(desiredFinalVolume, standardFadeDuration));
    }
    public void SetVoulme(float volume)
    {
        audioSource.volume =volume;
    }

    public IEnumerator TransitionRoutine(AudioClip newClip, float fadeDuration, float aimedFinalVolume=-1)
    {
        float finalVolume;
        if(aimedFinalVolume ==-1)finalVolume = audioSource.volume;
        else finalVolume = aimedFinalVolume;

        if(fadeInProgress!=null) StopCoroutine(fadeInProgress);
        fadeInProgress =StartCoroutine(FadeRoutine(0,fadeDuration)) ;
        
        yield return new WaitForSeconds(fadeDuration);
        audioSource.clip =newClip;
        yield return new WaitForSeconds(fadeDuration);

        if(fadeInProgress!=null) StopCoroutine(fadeInProgress);
        fadeInProgress =StartCoroutine(FadeRoutine(finalVolume,fadeDuration));
    }
    IEnumerator FadeRoutine(float desiredFinalVolume,float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime<duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start,desiredFinalVolume, currentTime/duration);
            yield return null; 
        }
    }





}
