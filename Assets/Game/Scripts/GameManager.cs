using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityAction onGamePauses;
    public UnityAction onGameGoesCinematics;
    public UnityAction onGAmeGoesPlayMode;
    public static GameManager Instance{get; private set;}
    public GameState GameState { get => gameState; set => gameState = value; }
    [SerializeField] GameObject pauseMenu;
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
        
       
        Time.timeScale = 1;
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
        }
        
    }
    public void PlayGame()
    {
        UnPauseGame();
    }
    public void CinematicMode()
    {
        onGameGoesCinematics?.Invoke();
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
