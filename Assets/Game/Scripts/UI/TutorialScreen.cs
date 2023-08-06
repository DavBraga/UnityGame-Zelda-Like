using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialScreen : MonoBehaviour
{
    CanvasGroup myCanvasGroup;
    Coroutine fadeRoutine;

    bool done = false;

    private void Awake() {
        myCanvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(fadeRoutine!=null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(1,1f));
    }
    public void LeaveTutorial()
    {
        done = true;
        GameManager.Instance?.ChangeGameState(GameState.playing);
        if(fadeRoutine!=null) StopCoroutine(fadeRoutine);
        fadeRoutine= StartCoroutine(FadeRoutine(0,1f));
    }


    public IEnumerator FadeRoutine(float desiredFinalOpacity,float duration)
    {
        float currentTime = 0;
        float start = myCanvasGroup.alpha;
        while (currentTime<duration)
        {
            currentTime += Time.deltaTime;
            myCanvasGroup.alpha = Mathf.Lerp(start,desiredFinalOpacity, currentTime/duration);
            yield return null; 
        }
    }
    public void Close(InputAction.CallbackContext value)
    {
        if(value.performed)
            if(!done)
                LeaveTutorial();
    }
}
