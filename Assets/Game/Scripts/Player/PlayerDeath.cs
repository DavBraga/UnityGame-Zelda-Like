using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent onDeath;
    [SerializeField]FadeEffect fader;
    [SerializeField] GameObject deadCamera;

    public delegate bool TakeDamageDelegate(GameObject attacker, int value);
    public TakeDamageDelegate onTakeDamage;
    public UnityAction onTakeDamageAction;
    Health health;
    
    CheckPointManager checkPointManager;

    Coroutine playerDeathRoutine;
    PlayerAvatar player;

    private void Awake() {
        checkPointManager =GetComponent<CheckPointManager>();
        health = GetComponent<Health>();
        player = GetComponent<PlayerAvatar>();
        GetComponent<PlayerAvatar>().onDeath+=PlayPlayerDeath;
    }

    private void OnEnable() {
        player.onDeath+= PlayerDie;
        player.onRessurect+= Ressurect;
        player.onPlayerTakeDamage+= TakeDamage;
    }
    private void OnDisable() {
        player.onDeath-= PlayerDie;
        player.onRessurect-= Ressurect;
        player.onPlayerTakeDamage-= TakeDamage;
    }
    public void PlayerDie()
    {
            Debug.Log("dies");
            if(player.stateMachine.currentState== player.deadState) return;

            Debug.Log("dies truly");
            player.animator.SetBool("bDead", true);
            player.HaltEverything();
            health.SetIgnoreDamage(true);
            //player.animator.SetBool("bDead", true);
            player.RemovePlayerControl();
            player.stateMachine.ChangeState(player.deadState);
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

    
    public bool TakeDamage(GameObject attacker,int damage)
    {   
        // check attacksuccess
        if(player.stateMachine.currentState!= player.hurtState)
        {
            bool returningValue = onTakeDamage.Invoke(attacker, damage);
            if(!returningValue) return false;
            onTakeDamageAction?.Invoke();
           // player.myRumbleManager.RumblePulse(.25f,.85f,hurtDuration*.65f);
            if(health.GetCurrentHealth()<=0)
            {
                PlayerDie(); 
            }
                
            return returningValue;   
        }
        return false;
    }
    public void Ressurect()
    {
        
        if(player.stateMachine.currentState == player.deadState)
        {
           // player.onRessurect?.Invoke();
            deadCamera.SetActive(false);
            StartCoroutine(WaitAndRess());
            
        }
    }
    IEnumerator WaitAndRess()
    {
        GetComponent<PlayerPhysics>().TurnOnPhysics();
        yield return new WaitForSeconds(2f); 
        player.stateMachine.ChangeState(player.idleState);
        player.animator.SetBool("bDead", false);
        yield return new WaitForSeconds(2f);
        
        health.SetHealth(0);
        health.Heal(Mathf.RoundToInt(health.GetMaxHealth()/2));
        health.SetIgnoreDamage(false);
        
        player.UnHaltEverything();
        player.TryGivePlayerControl();
    }
}
