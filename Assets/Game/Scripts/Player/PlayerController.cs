using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //coupling issues

    //PlayerController does too much:


    // - Interfaces player interaction
    // - Handles movement
    // - manages state machine
    // - interfaces animations, sounds
    // - manages attack action
    // - manages a variety of player actions(jump, open map,etc..)

    // plan of action:

    // Apply a simple componetization design where PlayerController will only interface player character actions interactions and game systems comunication. 
    // All other jobs player controller is doing will be separeted in different script componenets.
    
    //1- start at physics. FIRST ITERATION DONE
    //2- do a combat script. FIRST ITERATION DONE


    // how to integrate the player physics script

    // events, reference, message system

    public UnityAction onMap;
    public UnityAction onUnpause;
    public UnityAction onLossControl;
    public UnityAction onControlRecover;

    public boolConditionDelegate isGroundedDelegate;
    [SerializeField] bool startWithInputs = false;
    PlayerInput input;
    // state machine
    public StateMachine stateMachine{ get; private set;}
    public IdleState idleState{ get; private set;}
    public WalkingState walkingState{ get; private set;}
    public DeadState deadState{get; private set;}
    public OnAirState onAirState{ get; private set;}
    //public AttackState attackState{get; private set;}
    public AttackStateRedone attackState{get; private set;}
    public DefendState defendState{get; private set;}
    public HurtState hurtState{get; private set;}
    public Vector2 inputMovmentVector{ get; private set;}
    public Rigidbody myRigidbody{ get; private set;}
    public  Animator animator{ get; private set;}

    //private bool grounded= true;
    bool gotControl= true;
    public bool attacking = false;
    public bool defending = false;
    public UnityAction onInteractHook; 
    
    //GENERAL
    public Health health{get; private set;}
    public Collider thisCollider;
    BombTool bombTool;
    UsePotion usePotion;

    [Header("Movment")]
    [SerializeField] float acceleration = 10f;

    [Header("Jump and MidAir")]
    [SerializeField] float airMovmentSpeedModifier =0.25f;


    public bool forceNormalGravity = false;

    [Header("Hurt")]
    public float hurtDuration = 1f;

    [Header("DeadState")]
    [SerializeField]GameObject deadCamera;
    public UnityAction onDeath;

    [Header("Attack")]

    [SerializeField]float exitAttackCooldown =.3f;
    [HideInInspector]public float exitiAttackTime = 0;
    [SerializeField] float[] attackPreparationTime;
    public float[] attackDuration;
    public float[] attackCooldown;
    [SerializeField] float[] attackImpulse;

    [SerializeField] int attackPower = 1;

    [Header("Audio")]
    public SFXManager mySFXManager;
    public boolConditionDelegate damageConditions;

    [Header("Input")]
    ControllerRumbleManager myRumbleManager;

    [Header("Debug")]
    [SerializeField] string currentStateName;
    public int attackstage =0;
    // event interface
    public UnityAction onJump; 
    public UnityAction onMovmentInput;
    // physics
    public UnityAction<float> onMove, onRotate, onPlayerImpulse;
    // combat
    public delegate bool TakeDamageDelegate(GameObject attacker, int value);
    public TakeDamageDelegate onTakeDamage;
    public UnityAction onAttack, onDefend;
    public UnityAction<float,Vector3> onPushed;
    //public UnityAction<int> onTakeDamage;
    private void Awake()
    {
        InitializeStateMachine();
        input = GetComponent<PlayerInput>();
        if(!startWithInputs)
        input.DeactivateInput();
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();
        health = GetComponent<Health>();
        bombTool = GetComponent<BombTool>();
        usePotion = GetComponent<UsePotion>();
        mySFXManager = GetComponent<SFXManager>();
        myRumbleManager = GetComponent<ControllerRumbleManager>();
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
        GameManager.Instance.onGameGoesCinematics+=HaltEverything;
        GameManager.Instance.onGAmeGoesPlayMode += UnHaltEverything;

    }
    public void HaltEverything()
    {
        input.DeactivateInput();
        attacking = false;
        defending = false;
        inputMovmentVector = Vector2.zero;
    }
    public void UnHaltEverything()
    {
        // input.CancelInvoke();
        attacking = false;
        defending = false;
        inputMovmentVector = Vector2.zero;
        input.ActivateInput();
       
    }
    private void Update()
    {
        currentStateName = stateMachine.GetCurrentStateName();
        if(!inputMovmentVector.isZero()) onMovmentInput?.Invoke();
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        stateMachine.FixedUpdate();
    }
    private void LateUpdate() {
        stateMachine.LateUpdate();
    }
    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine();
        idleState = new IdleState(this);
        walkingState = new WalkingState(this);
        onAirState = new OnAirState(this, airMovmentSpeedModifier);
        //attackState = new AttackState(this, attackCollider);
        attackState = new AttackStateRedone(this);
        hurtState = new HurtState(this);
        deadState = new DeadState(this);
        stateMachine.ChangeState(idleState);
    }
    public void SetMovmentVector(Vector2 vector)
    {
        inputMovmentVector = vector;
    }
    public int GetCurrentHealth()
    {
        Debug.Log("Current health:"+ health.GetCurrentHealth());
        return health.GetCurrentHealth();
    }

    public void Die()
    {
        thisCollider.enabled= false;
        if(stateMachine.currentState!= deadState)
            stateMachine.ChangeState(deadState);
    }

    public void Ressurect()
    {
        thisCollider.enabled = true;
        myRigidbody.isKinematic = false;
        if(stateMachine.currentState == deadState)
        {
            deadCamera.SetActive(false);
            StartCoroutine(WaitAndRess());
        }
    }
    IEnumerator WaitAndRess()
    {
        yield return new WaitForSeconds(2f);
        stateMachine.ChangeState(idleState);
        yield return new WaitForSeconds(2f);
        
        
        health.Heal(Mathf.RoundToInt(health.GetMaxHealth()/2));
       
        health.SetIgnoreDamage(false);
        UnHaltEverything();
        TryGivePlayerControl();
    }
    //REACTION METHODS

    public bool TakeDamage(GameObject attacker,int damage)
    {   
        // check attacksuccess
        if(stateMachine.currentState!= hurtState)
        {
            bool returningValue = onTakeDamage.Invoke(attacker, damage);
            if(!returningValue) return false;
            myRumbleManager.RumblePulse(.25f,.85f,hurtDuration*.65f); 
            return returningValue;   
        }
        return false;
    }
    public bool ReadDefenseInput()
    {
        return defending;
    }

    public bool ReadAttackInput()
    {
        return attacking;
    }
    public Quaternion GetCameraForward()
    {
        float eulerY =Camera.main.transform.eulerAngles.y; 
        return Quaternion.Euler(0,eulerY,0);
    }

    public void PlayAttackAnimation(string attackTriggerTag = "tAttack1")
    {
        animator.SetTrigger(attackTriggerTag);
    }
    public Vector3 InputToV3()
    {
        return new Vector3(inputMovmentVector.x,0, inputMovmentVector.y);
    }
    public bool TryGivePlayerControl()
    {
         gotControl = true;
         onControlRecover?.Invoke();
         return gotControl;
    }
    public void RemovePlayerControl()
    {
        gotControl = false;
        onLossControl?.Invoke();
    }
    public void TryJump(bool jumpTrigger)
    {
        if(!jumpTrigger) return;
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        onJump?.Invoke();
    }
    public void TryAttack(bool attackTrigger)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(gotControl||attackTrigger) onAttack.Invoke();
        attacking = attackTrigger;
    }
    public void TryBlock(bool blockTrigger)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        defending = blockTrigger;
        if(!gotControl) return; 
        onDefend.Invoke();
    }
    public void TryUseTool()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(defending) return;
        bombTool.PutBomb(); 
    }
    public void TryUsePotion()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        usePotion.Use(); 
    }
    public void TryToInteract()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        onInteractHook?.Invoke();
    }
    public void TryEnlargeMap(bool enlargeTrigger)
    {
        if(!enlargeTrigger) return;
        if(!gotControl) return;
        onMap?.Invoke();
    }
    public void IncreaseAttackPower(int amount =1)
    {
        attackPower+=amount;
    }
}
