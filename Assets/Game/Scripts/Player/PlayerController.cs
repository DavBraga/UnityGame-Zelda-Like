using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public UnityAction onMap;
    public UnityAction onUnpause;
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

    Pushable pushable;

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

    float fVelocityRate;

    [Header("Movment")]
    [SerializeField] float acceleration = 10f;
    [SerializeField] float maxSpeed = 10f;

    Bounds bounds;
    
    [SerializeField] bool SnapStop = false;
    [Tooltip("Needs Snap Stop = true to have any effect.")]
    [SerializeField] float ExtraStopForce=5f;

    //[SerializeField] LayerMask collisionsLayer;

    [Header("Slope")]
    [SerializeField]float slopeAngle=0;
    [SerializeField]float  MaxSlopeAngle= 45;
    private Vector3 slopeNormal;

    Vector3 gravity;
    
    [Header("Jump and MidAir")]
    [SerializeField] float jumpPower=10f;
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

    [SerializeField] GameObject attackCollider;
    [SerializeField] GameObject shieldCollider;
    [SerializeField] GameInput swordHitVfx;
    [SerializeField] GameObject shieldBlockVfx;
    [SerializeField] float[] attackPreparationTime;
    public float[] attackDuration;
    public float[] attackCooldown;
    [SerializeField] float[] attackImpulse;

    [SerializeField] int attackPower = 1;
    [SerializeField] float knockbackPower;
    [SerializeField] float shieldKnocBack;
    [SerializeField] float inputCooldown=.001f;

    [Header("Shield Block")]
    [SerializeField]ShieldBlock shieldBlock;
    [SerializeField] GameObject attackFX;

    [Header("Audio")]
    public SFXManager mySFXManager;
    public boolConditionDelegate damageConditions;
    bool canttrigger= true;

    [Header("Debug")]
    [SerializeField] string currentStateName;

    [SerializeField] float currentVelocity;
    public int attackstage =0;

    private void Awake()
    {
        InitializeStateMachine();
        input = GetComponent<PlayerInput>();
        input.DeactivateInput();
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();
        health = GetComponent<Health>();
        bombTool = GetComponent<BombTool>();
        usePotion = GetComponent<UsePotion>();
        shieldBlock = GetComponent<ShieldBlock>();
        mySFXManager = GetComponent<SFXManager>();
        pushable = GetComponent<Pushable>();
        bounds = thisCollider.bounds;
        SetUpWeaponColliders();
    }

    private void OnDestroy() {
        GameManager.Instance?.RemovePlayer(this);
    }

    private void OnDisable() {
         GameManager.Instance?.RemovePlayer(this);
    }

    private void SetUpWeaponColliders()
    {
        attackCollider.GetComponent<WeaponCollision>().onHit += (collider) => { NewAttackTrigger(collider, attackPower, knockbackPower); };
        shieldCollider.GetComponent<WeaponCollision>().onHit += (collider) => { NewAttackTrigger(collider, 0, shieldKnocBack); };
        attackCollider.SetActive(false);
        shieldCollider.SetActive(false);
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
        // input.CancelInvoke();
        input.DeactivateInput();
        // input.CancelInvoke();
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
        CalculateVelocityRate();  
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        SufferGravity();
        if(GameManager.Instance.GameState != GameState.playing) return;
        stateMachine.FixedUpdate();
        //LimitMovmentSpeed();
        if(!SnapStop) return;
        if(inputMovmentVector.isZero())StopFaster(ExtraStopForce);
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
        attackState = new AttackStateRedone(this, attackCollider);
        defendState = new DefendState(this, shieldCollider);
        hurtState = new HurtState(this);
        deadState = new DeadState(this);
        stateMachine.ChangeState(idleState);
    }


  
    public void PlayerMovment(float intensity =1)
    {
        if(!gotControl) return; 
        Vector3 movmentVector = InputToV3();
        movmentVector = movmentVector.normalized;
        movmentVector = GetCameraForward()* movmentVector ;
        if(!forceNormalGravity)
        movmentVector = Vector3.ProjectOnPlane(movmentVector, slopeNormal);
        float maxDistance= bounds.size.z;//+.1f;
        Vector3 castOrigin =transform.position- new Vector3(0,0,bounds.size.z*.5f);

        bool isTouchingSomethingAhead ;
        //bool isTouchingSomethingAhead = Physics.Raycast(transform.position,movmentVector*maxDistance,collisionsLayer);
        isTouchingSomethingAhead = Physics.SphereCast(castOrigin,bounds.size.z*.4f,movmentVector,out _, maxDistance,GameManager.Instance.GetCollisionLayer())||
        Physics.SphereCast(castOrigin+ new Vector3(0,bounds.size.y,bounds.size.z*.5f),bounds.size.z*.4f,movmentVector,out _, maxDistance,GameManager.Instance.GetCollisionLayer());

             if(!IsGrounded()&&isTouchingSomethingAhead)
             {
                Debug.Log("touching");
                return;
             } 
        Debug.DrawRay(castOrigin,movmentVector*2,Color.red,.01f);
        movmentVector *= GetMovmentSpeed()*intensity;
        myRigidbody?.AddForce(movmentVector,ForceMode.Force);
    }

    public void Jump()
    {
        if(!IsGrounded())  return;
        if(!gotControl) return; 
        animator.SetTrigger("tJump");
        Debug.Log("Jumps");
        myRigidbody.AddForce((Vector3.up *jumpPower)-gravity.normalized, ForceMode.Impulse);
       // myRigidbody.AddForce(-gravity.normalized *jumpPower, ForceMode.Impulse);
    }

    public bool KeepChooping()
    {   
        Debug.Log("keep chopping");
        if(!gotControl|| GameManager.Instance.GameState!= GameState.playing) return false; 
        //
       if(Time.time<exitiAttackTime) return false;
        //if(!inputMovmentVector.isZero()) return false;

        if(stateMachine.currentState!=attackState)
            stateMachine.ChangeState(attackState);

        return true;   
    }
    public void Defend()
    {
        if(!gotControl) return; 
        if(stateMachine.currentState!=defendState)
            stateMachine.ChangeState(defendState);
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
            Debug.Log("ressurected");
            deadCamera.SetActive(false);
            
            StartCoroutine(WaitAndRess());
        }
        
    }

    IEnumerator WaitAndRess()
    {
        yield return new WaitForSeconds(2f);
        stateMachine.ChangeState(idleState);
        //health.SetIgnoreDamage(true);
        yield return new WaitForSeconds(2f);
        
        
        health.Heal(Mathf.RoundToInt(health.GetMaxHealth()/2));
       
        health.SetIgnoreDamage(false);
        UnHaltEverything();
        TryGivePlayerControl();
    }
    private void SufferGravity()
    {
        if(forceNormalGravity) gravity = Physics.gravity;
        else gravity = DetectOnSlope() ? slopeNormal * Physics.gravity.y : Physics.gravity;
        Debug.DrawRay(transform.position, gravity.normalized * 2, Color.blue, .1f);
        myRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }

    //REACTION METHODS

    public bool TakeDamage(GameObject attacker, int damage)
    {
        if(stateMachine.currentState == deadState) return false;
        if(stateMachine.currentState== defendState&& !shieldBlock.DirectionCanDealDamage(attacker)) return false;
       
        // check attacksuccess
        if(stateMachine.currentState!= hurtState)
        {
           if(!health.TakeDamage(attacker, damage)) return false;

            if(health.GetCurrentHealth()<1)
            {
                stateMachine.ChangeState(deadState);
                return true;
            }  
            stateMachine.ChangeState(hurtState);   
        }
        return false;
    }
    public void BePushed(GameObject pusher,float pushPower, Vector3 direction)
    {
        if(stateMachine.currentState== defendState&&!shieldBlock.DirectionCanDealDamage(pusher)) return;
        pushable.BePushed(pushPower, direction);
    }
    //SUPPORT METHODS
    private void CalculateVelocityRate()
    {
        fVelocityRate = 0;
        if (!inputMovmentVector.isZero())
        {
            fVelocityRate = myRigidbody.velocity.magnitude/maxSpeed;
        }
        animator.SetFloat("fVelocity", fVelocityRate);
    } 
    private void StopFaster(float stopPower=1f)
    {
        if(!IsGrounded()) return;
        if(Mathf.Abs(myRigidbody.velocity.magnitude)<.5f) return;
        myRigidbody.AddForce(-myRigidbody.velocity.normalized * stopPower);
    }
    public void LimitMovmentSpeed(float factor=1)
    {
        Vector3 horizontalVelocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
        if (horizontalVelocity.magnitude > (maxSpeed*factor))
        {
            horizontalVelocity = horizontalVelocity.normalized * (maxSpeed*factor);
            myRigidbody.velocity = new Vector3(horizontalVelocity.x, myRigidbody.velocity.y, horizontalVelocity.z);
        }
    }
    
    public void  RotateBodyToFace(float smoothness = .15f){

        if(inputMovmentVector.isZero()) return;

        Quaternion q1 = Quaternion.LookRotation(InputToV3(),Vector3.up); 
        Quaternion q2 = GetCameraForward();
        Quaternion toRotation = q1*q2;
        Quaternion smoothRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation,smoothness);
        myRigidbody.MoveRotation(smoothRotation);

    }
    public bool ReadDefenseInput()
    {
        return defending;
    }

    public bool ReadAttackInput()
    {
        return attacking;
    } 
    public float GetMovmentSpeed()
    {
        return acceleration;
    }
    public Quaternion GetCameraForward()
    {
        float eulerY =Camera.main.transform.eulerAngles.y; 
        return Quaternion.Euler(0,eulerY,0);
    }

    public void PlayAttackAnimation(int attackStage=0)
    {
        animator.ResetTrigger("tAttack1");
        animator.ResetTrigger("tAttack2");
        animator.ResetTrigger("tAttack3");
            
            if(attackStage==0 ||attackStage>3)
            {
                animator.SetTrigger("tAttack1");
            }
                
            else if(attackStage==1)
            {
                animator.SetTrigger("tAttack2");

            }

            else if(attackStage==2)
            {
                animator.SetTrigger("tAttack3");

            }
            else if(attackStage==3) animator.SetTrigger("tAttack4");

    }

    public void PlayAttackImpulse(int attackStage)
    {
        myRigidbody.AddForce(transform.forward*attackImpulse[attackStage] ,ForceMode.Impulse);
    }

    public void NewAttackTrigger(Collider other, int atkpower =1, float pushPower = 5f)
    {
        // apply damage
        //apply push

        if(other.TryGetComponent(out CreatureController creatureController))
        {
            creatureController.TakeDamage(this.gameObject,atkpower);
        }
        if(other.TryGetComponent(out Pushable pushable))
        {
            var positionDiff = other.gameObject.transform.position - transform.position;
            positionDiff.Normalize();
            pushable.BePushed(pushPower,positionDiff);
        }

    }
    public float[] GetAttackPreparationTIme()
    {
        return attackPreparationTime;
    }
    public Vector3 InputToV3()
    {
        return new Vector3(inputMovmentVector.x,0, inputMovmentVector.y);
    }

    public bool IsGrounded()
    {
        Vector3 direction = Vector3.down;
        Bounds bounds = thisCollider.bounds;
        Vector3 origin = transform.position + new Vector3(0,bounds.size.y,0);
        float radius = bounds.size.x * 0.33f;
        float maxDistance = bounds.size.y+0.1f;
        //Vector3 spherePosition = direction* maxDistance + origin;
        LayerMask groundLayer = GameManager.Instance.GetGroundLayer();
        if (Physics.SphereCast (origin,radius,direction,out _,maxDistance,groundLayer))
        {
            animator.SetBool("bOnAir", false);
                return true;      
        }
        animator.SetBool("bOnAir", true);
        return false;
    }

    public bool DetectOnSlope()
    {
        Bounds bounds = thisCollider.bounds;
        Vector3 origin = transform.position + new Vector3(0,bounds.size.y,0);
        Vector3 direction = Vector3.down;
        float maxDistance = bounds.size.y+.1f;
        LayerMask groundLayer = GameManager.Instance.GetGroundLayer();
        if(Physics.Raycast(origin, direction,out var slopeHitInfo,maxDistance,groundLayer))
        {
            slopeAngle = Vector3.Angle(Vector3.up,slopeHitInfo.normal);
            slopeNormal = slopeHitInfo.normal;
            return slopeAngle<MaxSlopeAngle&& slopeAngle !=0;
        }

        return false;

    }
    public bool TryGivePlayerControl()
    {
       // UnHaltEverything();
        // if(stateMachine.currentState== hurtState) return false;
        // if(stateMachine.currentState == deadState) return false;
         gotControl = true;
         return gotControl;
        
    }

    public void RemovePlayerControl()
    {
        gotControl = false;
        
    }
    public void SetMovment(InputAction.CallbackContext value)
    {
        inputMovmentVector = value.ReadValue<Vector2>();
    }

    public void SetUpJump(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(value.performed)
         Jump(); 
    }
    public void SetAttack(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        //if(!gotControl) return;

        // if(value.started)
        if(value.performed)
        {
            KeepChooping();
            Debug.Log("doing");
        }
        
        attacking = !value.canceled;
        
        if(value.canceled) 
        {
            Debug.Log("cancelled");
            return;
        }
        
    }
    public void SetBlock(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
       // if(!gotControl) return;

        defending = !value.canceled;
        Defend();
    }

    public void SetUseTool(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(defending) return;
        if(value.performed)
        bombTool.PutBomb(); 
    }

    public void SetUsePotion(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(value.performed)
        usePotion.Use(); 
    }
    public void SetUseInteraction(InputAction.CallbackContext value)
    {
        if(GameManager.Instance.GameState != GameState.playing) return;
        if(!gotControl) return;
        if(value.performed)
        onInteractHook?.Invoke();
    }
    public void SetPauseGame(InputAction.CallbackContext value)
    {
         
        if(value.performed)
            GameManager.Instance.TogglePause();
        
    }

    public void SetMap(InputAction.CallbackContext value)
    {
        if(!gotControl) return;
        onMap?.Invoke();
    }
    public void IncreaseAttackPower(int amount =1)
    {
        attackPower+=amount;
    }
    // For test purpose Only
    // private void OnDrawGizmos() {

        
    //     Vector3 direction = Vector3.down;
    //     Bounds bounds = thisCollider.bounds;
    //     Vector3 origin = transform.position + new Vector3(0,bounds.size.y,0);
    //     float radius = bounds.size.x * 0.33f;
    //     float maxDistance = bounds.size.y;// *0.3f;
    //     Vector3 spherePosition = direction* maxDistance + origin;

    //     Gizmos.color = grounded? Color.green : Color.red;
    //     Gizmos.DrawSphere(spherePosition, radius );
    // }
}
