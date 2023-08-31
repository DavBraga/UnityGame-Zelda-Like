using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField]PlayerController controller;
    public boolConditionDelegate isGroundedDelegate;
    public  Animator Animator{ get; private set;}
    public bool attacking = false;
    public bool defending = false;
    public UnityAction onInteractHook; 

    public UnityAction onDeath,onRessurect;
    public UnityAction<PowerUpType> OnPowerUp;

    [Header("Audio")]
    public SFXManager mySFXManager;
    public int attackstage =0;
    // event interface
    public UnityAction onJump; 
    public UnityAction onStateInitializationFinished;
    // physics
    public UnityAction<float> onMove, onRotate, onPlayerImpulse;
    public UnityAction onAir, onLand;
    // combat
    public delegate bool TakeDamageDelegate(GameObject attacker, int value);
    public TakeDamageDelegate onPlayerTakeDamage;
    public UnityAction onPowerIncrease; 
    public UnityAction<GameObject,float,Vector3> onCombatPushed;
    public UnityAction<float,Vector3> onPushed;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        mySFXManager = GetComponent<SFXManager>();
    }
    private void OnDestroy() {
        GameManager.Instance?.RemovePlayer(this);
    }
    private void OnDisable() {
         GameManager.Instance?.RemovePlayer(this);
    }
    private void Start() {
        StartCoroutine(SubscribleToGameManager()); 
    }
    IEnumerator SubscribleToGameManager()
    {
        yield return new WaitUntil(()=>GameManager.IsManagerReady());
        GameManager.Instance.SetPlayer(this);

    }
    public Quaternion GetCameraForward()
    {
        float eulerY =Camera.main.transform.eulerAngles.y; 
        return Quaternion.Euler(0,eulerY,0);
    }

    public void PlayAttackAnimation(string attackTriggerTag = "tAttack1")
    {
        Animator.SetTrigger(attackTriggerTag);
    }
    public PlayerController GetPlayerController()
    {
        return controller;
    }
}
