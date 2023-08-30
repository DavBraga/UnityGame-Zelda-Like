using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //to import from PlayerAvatar:
    // inputs, logic;
    PlayerInput input;
    public UnityAction onMap,onInteractHook;
    public UnityAction onMovmentInput;
    UnityAction onControlRecover, onLossControl;
    public UnityAction onStateInitializationFinished;

    public float hurtDuration = 1f;

    [SerializeField] PlayerAvatar controlledAvatar;

    public Vector2 inputMovmentVector{ get; private set;}
    // combat
    public delegate bool TakeDamageDelegate(GameObject attacker, int value);
    public TakeDamageDelegate onPlayerTakeDamage;
    public UnityAction onAttack, onDefend, onPowerIncrease; 
    public UnityAction<Transform> onUseTool; 
    public UnityAction<Transform> onUsePotion;

    public UnityAction onDeath;

    bool gotControl=true;
    public bool attacking;
    public bool defending;

    //stateMachine
    public StateMachine stateMachine{ get; private set;}
    public IdleState idleState{ get; private set;}
    public WalkingState walkingState{ get; private set;}
    public DeadState deadState{get; private set;}
    public OnAirState onAirState{ get; private set;}
    public AttackStateRedone attackState{get; private set;}
    public DefendState defendState{get; private set;}
    public HurtState hurtState{get; private set;}
    //public Vector2 inputMovmentVector{ get; private set;}
    //public  Animator animator{ get; private set;}

    [HideInInspector]public float exitiAttackTime = 0;

    private void Awake() {
        input = GetComponent<PlayerInput>();
    }

    private void Start() {
        InitializeStateMachine();
        controlledAvatar.onPlayerTakeDamage+= onPlayerTakeDamage.Invoke;  
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

    public void SetMovmentVector(Vector2 vector)
    {
        inputMovmentVector = vector;
    }

     public Vector3 InputToV3()
    {
        return new Vector3(inputMovmentVector.x,0, inputMovmentVector.y);
    }

    public bool ReadDefenseInput()
    {
        return defending;
    }

    public bool ReadAttackInput()
    {
        return attacking;
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
    public void TryAttack(bool attackTrigger)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(gotControl||attackTrigger) onAttack.Invoke();
        attacking = attackTrigger;
    }
    public void TryBlock(bool blockTrigger)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(gotControl||defending) onDefend.Invoke();
        defending = blockTrigger;
        
    }
    public void TryUseTool()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(defending) return;
        onUseTool?.Invoke(controlledAvatar.transform); 
    }
    public void TryUsePotion()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        onUsePotion?.Invoke(controlledAvatar.transform); 
    }
    public void TryEnlargeMap(bool enlargeTrigger)
    {
        if(!enlargeTrigger) return;
        if(!gotControl) return;
        onMap?.Invoke();
    }

    public void TryToInteract()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        controlledAvatar.onInteractHook?.Invoke();
    }

    public void TryJump(bool jumpTrigger)
    {
        
        if(!jumpTrigger) return;
        if(GameManager.Instance.GameState != GameState.playing) return;
        
        if(!gotControl) return;
        Debug.Log("try jump:"+ jumpTrigger);
        controlledAvatar.onJump?.Invoke();
    }

    public PlayerAvatar GetControlledAvatar()
    {
        return controlledAvatar;
    }

       private void FixedUpdate()
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        stateMachine.FixedUpdate();
    }
    private void LateUpdate() {
        stateMachine.LateUpdate();
    }
     private void Update()
    {
        //currentStateName = stateMachine.GetCurrentStateName();
        if(!inputMovmentVector.isZero()) onMovmentInput?.Invoke();
        stateMachine.Update();
    }
}
