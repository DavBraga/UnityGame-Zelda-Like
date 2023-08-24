using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainScreen : MonoBehaviour
{
    public UnityEvent onFireGame;
    public UnityEvent onGameStarts;

    [SerializeField] string creditsScreen = "Endgame";

    [Header("LoadGame")]
    [SerializeField] private string GameScene;
    [SerializeField] float delay = 3f;

    [SerializeField] BarHandler loadbar;

    

    bool gameStarted= false;
    AudioManager audioManager;

    private void Awake() {
        audioManager = GetComponent<AudioManager>();
        onFireGame.AddListener(()=>{ audioManager.FadeVolume(0,delay);});
    }
    // Start is called before the first frame update
    void Start()
    {
        onGameStarts?.Invoke();
    }
    public void StartGame()
    {
        if(!gameStarted)
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        gameStarted = true;
        yield return new WaitForSeconds(delay);
        //SceneManager.LoadScene(GameScene);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loadbar.gameObject.SetActive(true);
        AsyncOperation asyncSceneload = SceneManager.LoadSceneAsync(GameScene);
        while(!asyncSceneload.isDone)
        {
            loadbar.UpdateBarValue(Mathf.Clamp01(asyncSceneload.progress/.9f));
            yield return null;
        }
        
    }

     public void StartGame(InputAction.CallbackContext value)
    {
        if(value.performed)
        onFireGame?.Invoke();
    }

    public void Credits(InputAction.CallbackContext value)
    {
        if (value.performed)
        CallCredits();
    }

    public void CallCredits()
    {
            SceneManager.LoadScene(creditsScreen);
    }

}
