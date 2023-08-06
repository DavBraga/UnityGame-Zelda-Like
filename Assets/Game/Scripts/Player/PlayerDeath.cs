using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent onDeath;
    [SerializeField]FadeEffect fader;
    
    CheckPointManager checkPointManager;

    Coroutine playerDeathRoutine;

    private void Awake() {
        checkPointManager =GetComponent<CheckPointManager>();
        GetComponent<PlayerController>().onDeath+=PlayPlayerDeath;
    }
    public void PlayPlayerDeath()
    {
        if(playerDeathRoutine!=null) StopCoroutine(playerDeathRoutine);
        StartCoroutine(PlayDeathEvent());
    }
    IEnumerator PlayDeathEvent()
    {
        onDeath?.Invoke();

        GameManager.Instance?.ChangeGameState(GameState.pause);
        yield return new WaitForSeconds(2.5f);
        fader.FadeOut();
        yield return new WaitForSeconds(1.5f);
        checkPointManager.RestorePlayer();
        
    }
}
