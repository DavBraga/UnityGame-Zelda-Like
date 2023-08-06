using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
   // Animator animator;
    CanvasGroup canvasGroup;
    Coroutine fadeInProgress;

    Image fadeImg;
    Coroutine fadingRoutine;

    [Header("Start with a fade in?")]
    [SerializeField]bool startWithAfadeIn = false;
    [SerializeField] float delay =1f;
    [SerializeField] float standardFadeDuration=1f;
    private void Awake() {
        //animator =GetComponent<Animator>();
        fadeImg = GetComponent<Image>();
        canvasGroup =GetComponent<CanvasGroup>();
        if(startWithAfadeIn)
         canvasGroup.alpha = 1;
           
    }
    private void Start() {
        if(startWithAfadeIn)
            if(delay>0)DelayedFadeIN(delay,standardFadeDuration);
            else FadeIn(standardFadeDuration);
    }

    public void FadeIn()
    {
        Fade(0f,standardFadeDuration);
    }
       public void FadeIn(float fadeDuration)
    {
        Fade(0f,fadeDuration);
    }
    public void FadeOut()
    {
        Fade(1f,standardFadeDuration);
    }
     public void FadeOut(float fadeDuration)
    {
        Fade(1f,fadeDuration);
    }

    public void DelayedFadeIN(float timeDelay, float fadeDuration)
    {
        if(fadeInProgress==null)
        fadeInProgress =StartCoroutine(DelayedFade(0f,timeDelay,fadeDuration));
    }
     public void DelayedFadeOut(float timeDelay)
    {
        if(fadeInProgress==null)
        fadeInProgress =StartCoroutine(DelayedFade(1f,timeDelay,standardFadeDuration));
    }

    IEnumerator DelayedFade(float finalOpacitiy, float waitTime, float fadeDuration)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Done waiting");
        Fade(finalOpacitiy, fadeDuration);
        fadeInProgress = null;
    }

    public void Fade(float finalOpacitiy, float duration)
    {
        if(fadingRoutine!=null) StopCoroutine(fadingRoutine);
            fadingRoutine = StartCoroutine(FadeRoutine(finalOpacitiy,duration));

    }

    public void ChageFadeColor(Color color )
    {
        fadeImg.color = Color.white;

    }


    public IEnumerator FadeRoutine(float desiredFinalOpacity,float duration)
    {
        float currentTime = 0;
        float start = canvasGroup.alpha;
        while (currentTime<duration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start,desiredFinalOpacity, currentTime/duration);
            yield return null; 
        }
    }

    public IEnumerator FadeColorRoutine(Color finalColor,float duration)
    {
        float currentTime = 0;
        float start = canvasGroup.alpha;
        while (currentTime<duration)
        {
            currentTime += Time.deltaTime;
           // canvasGroup.alpha = Mathf.Lerp(start,desiredFinalOpacity, currentTime/duration);
            yield return null; 
        }
    }
}
