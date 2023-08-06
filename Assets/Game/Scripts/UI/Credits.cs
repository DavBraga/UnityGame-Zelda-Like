using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour
{
    [SerializeField] private RollUpELement[] elements;
    [SerializeField] FadeEffect WhiteFader;
    [SerializeField]GameObject BlackFader;

    [SerializeField] AudioManager music;
    [SerializeField] string mainScreen ="MainMenu";
    bool ended = false;

    public UnityEvent onCrerditisStart;
    [SerializeField] float endDelay = 2f;
    [SerializeField] string standardLeaveTrigger = "tLeave";
    [SerializeField] float intervals = 2f;
    int listPointer = 0;

    private void Start() {
        onCrerditisStart?.Invoke();
        RollUp();
    }

    public void RollUp()
    {
        StartCoroutine(HoldOnAndMove(elements[listPointer].holdTime));
    }

    IEnumerator HoldOnAndMove(float holdTime)
    {
        yield return new WaitForSeconds(holdTime);
        if(listPointer>=elements.Length)
        {
            Finish();
            yield break;
        } 
        elements[listPointer].animator.SetTrigger(standardLeaveTrigger);

        yield return new WaitForSeconds(intervals);
        elements[listPointer].animator.gameObject.SetActive(false);
        listPointer++;
        if(listPointer<elements.Length)
        {
            elements[listPointer].animator.gameObject.SetActive(true);
            RollUp();
        }
        
        else
        Finish();
    }

    private void Finish()
    {
        if(ended) return;
        ended = true;
        music.FadeVolume(0,3f);
        WhiteFader.FadeOut();
        StartCoroutine(WaitAndRestart());
        
    }
    IEnumerator WaitAndRestart()
    {
        yield return new WaitForSeconds(endDelay);
        BlackFader.SetActive(true);
        WhiteFader.FadeIn();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(mainScreen);
    }
       public void SetPauseGame(InputAction.CallbackContext value)
    {
        if(value.performed)
        Finish();
    }


}


    [Serializable]
    public struct RollUpELement
    {
        public Animator animator;
        public float holdTime;
        public string leaveTrigger;
    }
