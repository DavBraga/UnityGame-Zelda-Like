using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public UnityAction onGamePauses;
    public UnityAction onGameGoesCinematics;
    public UnityAction onGAmeGoesPlayMode;
    public static GameManager Instance{get; private set;}
    public GameState GameState { get => gameState; set => gameState = value; }
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject virtualInput;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] GameState gameState;


   [SerializeField] string creditsScene;

    //  TODO improve it
    [SerializeField] GameObject player;
    bool gotPlayer= false;
    static bool ManagerIsReady = false;

    private void OnDisable() {
        if(Instance == this)
        ManagerIsReady = false;
    }
    private void OnEnable() {
        if(Instance== this)
        ManagerIsReady = true;
    }
    private void OnDestroy() {
        if(Instance == this)
        ManagerIsReady = false;
    }

    private void Awake() {
        if(Instance && Instance!=this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        ManagerIsReady = true;
    }
    static public bool IsManagerReady()
    {
        return ManagerIsReady;
    }
    // Update is called once per frame
   public void ReloadLevel()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    public LayerMask GetGroundLayer()
    {
        return groundLayer;
    }
    public LayerMask GetCollisionLayer()
    {
        return collisionLayer;
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player.gameObject;
        if(player!=null)
        gotPlayer = true;
    }

    public bool RemovePlayer(PlayerController player)
    {
        if(this.player==player)
        {
            this.player = null;
           // if(player==null)
            gotPlayer = false;
            return true;
        }
        return false;
    }
    public bool CheckForPlayer()
    {
        return gotPlayer;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void ChangeGameState(GameState state)
    {
        GameState = state;
    }

    public void PauseGame()
    {
        if(gameState!= GameState.cinematic)
        
        onGamePauses?.Invoke();
        
        ChangeGameState(GameState.pause);
        Time.timeScale = 0;
        
    }

    public void UnPauseGame()
    { 
        ChangeGameState(GameState.playing);
        onGAmeGoesPlayMode?.Invoke();
        if(Application.isMobilePlatform||Application.platform==RuntimePlatform.WebGLPlayer)
        {
             if(PlayerPrefs.GetInt("usingVirtualInput")>0)
            virtualInput.SetActive(true);
        }
       
        
       
        Time.timeScale = 1;
        PlayerPrefs.Save();
    }

    public void TogglePause()
    {  
        if(gameState == GameState.pause)
        {
            UnPauseGame();
            pauseMenu.SetActive(false);
            
        }
        else
        {
            PauseGame();
            pauseMenu.SetActive(true);
            virtualInput.SetActive(false);
        }
        
    }
    public void PlayGame()
    {
        UnPauseGame();
    }
    public void CinematicMode()
    {
        onGameGoesCinematics?.Invoke();
        virtualInput.SetActive(false);
        ChangeGameState(GameState.cinematic);
    }
    public void GoCredits()
    {
        SceneManager.LoadScene(creditsScene);
    }
}



   public enum GameState
    {
        playing,
        pause,
        cinematic
    }
