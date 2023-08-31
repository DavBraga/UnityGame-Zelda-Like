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

    public UnityAction<PowerUpType> onPowerUp;
    public UnityAction<ItemSO> onInventoryUpgrade;

    public UnityAction onDeath;

    bool gotControl=true;
    public bool attacking;
    public bool defending;

    //stateMachine
    public StateMachine StateMachine{ get; private set;}
    public IdleState IdleState{ get; private set;}
    public WalkingState WalkingState{ get; private set;}
    public DeadState DeadState{get; private set;}
    public OnAirState OnAirState{ get; private set;}
    public AttackStateRedone AttackState{get; private set;}
    public DefendState DefendState{get; private set;}
    public HurtState HurtState{get; private set;}

    [HideInInspector]public float exitiAttackTime = 0;

    private void Awake() {
        input = GetComponent<PlayerInput>();
    }

    private void Start() {
        InitializeStateMachine();
        controlledAvatar.onPlayerTakeDamage+= onPlayerTakeDamage.Invoke; 
        controlledAvatar.OnPowerUp+= onPowerUp.Invoke;
        controlledAvatar.onDeath+= onDeath.Invoke; 
    }

    public void AttachAvatar(PlayerAvatar avatar)
    {
        controlledAvatar = avatar;
        controlledAvatar.onPlayerTakeDamage+= onPlayerTakeDamage.Invoke; 
        controlledAvatar.OnPowerUp+= onPowerUp.Invoke;
         controlledAvatar.onDeath+= onDeath.Invoke; 
    }
    public PlayerAvatar DetachCurrentAvatar()
    {
        PlayerAvatar detachingAvatar = controlledAvatar;
        controlledAvatar = null;
        controlledAvatar.onPlayerTakeDamage-= onPlayerTakeDamage.Invoke; 
        controlledAvatar.OnPowerUp-= onPowerUp.Invoke;
         controlledAvatar.onDeath-= onDeath.Invoke; 
        return detachingAvatar;
    }
    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();
        IdleState = new IdleState(this);
        WalkingState = new WalkingState(this);
        OnAirState = new OnAirState(this);
        AttackState = new AttackStateRedone(this);
        HurtState = new HurtState(this);
        DeadState = new DeadState(this);
        DefendState = new DefendState(this);

        onStateInitializationFinished?.Invoke();
        StateMachine.ChangeState(IdleState);
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
        StateMachine.FixedUpdate();
    }
    private void LateUpdate() {
        StateMachine.LateUpdate();
    }
     private void Update()
    {
        if(!inputMovmentVector.isZero()) onMovmentInput?.Invoke();
        StateMachine.Update();
    }
}
