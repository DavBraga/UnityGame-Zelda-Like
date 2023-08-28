using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAvatar : MonoBehaviour
{
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
    public AttackStateRedone attackState{get; private set;}
    public DefendState defendState{get; private set;}
    public HurtState hurtState{get; private set;}
    public Vector2 inputMovmentVector{ get; private set;}
    public  Animator animator{ get; private set;}

    //private bool grounded= true;
    bool gotControl= true;
    public bool attacking = false;
    public bool defending = false;
    public UnityAction onInteractHook; 
    
    //GENERAL
    //public Health health{get; private set;}

    [Header("Hurt")]
    public float hurtDuration = 1f;
    public UnityAction onDeath,onRessurect;

    [Header("Attack")]
    [HideInInspector]public float exitiAttackTime = 0;

    [Header("Audio")]
    public SFXManager mySFXManager;
    public boolConditionDelegate damageConditions;

    [Header("Debug")]
    [SerializeField] string currentStateName;
    public int attackstage =0;
    // event interface
    public UnityAction onJump; 
    public UnityAction onMovmentInput;
    public UnityAction onStateInitializationFinished;
    // physics
    public UnityAction<float> onMove, onRotate, onPlayerImpulse;
    public UnityAction onAir, onLand;
    public UnityAction<bool> onRigidBodyChanges;
    // combat
    public delegate bool TakeDamageDelegate(GameObject attacker, int value);
    public TakeDamageDelegate onPlayerTakeDamage;
    public UnityAction onAttack, onDefend, onPowerIncrease; 
    public UnityAction<GameObject,float,Vector3> onCombatPushed;
    public UnityAction<float,Vector3> onPushed;
    // actions
    public UnityAction onUseTool, onUsePotion;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        if(!startWithInputs)
        input.DeactivateInput();
        animator = GetComponent<Animator>();
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
        InitializeStateMachine();   
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
        onAirState = new OnAirState(this);
        attackState = new AttackStateRedone(this);
        hurtState = new HurtState(this);
        deadState = new DeadState(this);
        defendState = new DefendState(this);

        onStateInitializationFinished?.Invoke();
        stateMachine.ChangeState(idleState);
    }
    public void SetMovmentVector(Vector2 vector)
    {
        inputMovmentVector = vector;
    }
    //REACTION METHODS
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
        if(gotControl||defending)onDefend.Invoke();
        defending = blockTrigger;
        
    }
    public void TryUseTool()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(defending) return;
        onUseTool?.Invoke(); 
    }
    public void TryUsePotion()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        onUsePotion?.Invoke(); 
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
}
