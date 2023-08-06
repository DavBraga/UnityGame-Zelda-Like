using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingCInematicSequence : MonoBehaviour
{
    //-> on death
    [SerializeField]FadeEffect fader;
    [SerializeField]AudioManager bossMusicManager;
    PlayableDirector director;

    private void Awake() {
        director = GetComponent<PlayableDirector>();
    }

    public void StartBossDeath()
    {
        GameManager.Instance.ChangeGameState(GameState.cinematic);
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {
        fader.Fade(1f,.3f);
        bossMusicManager.FadeVolume(0,3f);
        yield return new WaitForSeconds(2f);
        director.Play();
        fader.Fade(0f,1f);
    }

    private void OnEnable() {
        director.stopped += FinishEnding;
    }

    public void FinishEnding(PlayableDirector playableDirector)
    {
        StartCoroutine(GoCredits());
        
    }
    IEnumerator GoCredits()
    {
        fader.ChageFadeColor(Color.white);
        fader.Fade(1, 2f);
        yield return new WaitForSeconds (7f);
        GameManager.Instance?.GoCredits();
    }

    //start death:
    // fade out
    // fade music out
    // fade in to sequence:

    // -camera to boss
    // -vfx
    // - fade out
    // -> go credits
}
