using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // state machine
    public StateMachine stateMachine{ get; private set;}
    public IdleState idleState{ get; private set;}
    public WalkingState walkingState{ get; private set;}
    public DeadState deadState{get; private set;}
    public OnAirState onAirState{ get; private set;}
    public AttackState attackState{get; private set;}
    public DefendState defendState{get; private set;}

    public HurtState hurtState{get; private set;}
    public Vector2 inputMovmentVector{ get; private set;}
    public Rigidbody myRigidbody{ get; private set;}
    public  Animator animator{ get; private set;}

    private bool grounded= true;
    
    //GENERAL
    public Health health{get; private set;}

    [SerializeField] Collider thisCollider;

    [Header("Bombs")]

    [SerializeField] bool gotBombSkill= false;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombIntervals = 3f;
    bool canBomb = true;


    float fVelocityRate;

    [Header("Movment")]
    [SerializeField] float acceleration = 10f;
    [SerializeField] float maxSpeed = 10f;
    
    [SerializeField] bool SnapStop = false;
    [Tooltip("Needs Snap Stop = true to have any effect.")]
    [SerializeField] float ExtraStopForce=5f;

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

    [Header("Attack")]
    [SerializeField] GameObject attackCollider;
    [SerializeField] GameObject shieldCollider;
    [SerializeField] float[] attackGracePeriod;
    [SerializeField] float[] attackDuration;
    [SerializeField] float[] attackImpulse;
    [SerializeField] int attackPower = 1;
    [SerializeField] float knockbackPower;
    [SerializeField] float shieldKnocBack;
    [SerializeField] float inputCooldown=.001f;
    bool canttrigger= true;



    [Header("Debug")]
    [SerializeField] string currentStateName;

    [SerializeField] float currentVelocity;
    public int attackstage =0;

    private void Awake() {
        InitializeStateMachine();
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();
        health = GetComponent<Health>();
        if(health) health.onTakeDamage += onTakeDamage;
        
    }
    private void Start()
    {
       
    }
    private void Update()
    {
        currentStateName = stateMachine.GetCurrentStateName();
       if(stateMachine.currentState!=deadState&&stateMachine.currentState!=hurtState )
       {
           if(gotBombSkill) PutBomb();
            if (ReadLeftMouseInput()) Chop();
            if(ReadRightMouseInput()) stateMachine.ChangeState(defendState);
            inputMovmentVector = ReadMovmentInput();
            CalculateVelocityRate();  
        }
        stateMachine.Update();
        //currentVelocity = myRigidbody.velocity.magnitude;
        
    }
    private void FixedUpdate()
    {
        SufferGravity();
        stateMachine.FixedUpdate();
        LimitMovmentSpeed();
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
        attackState = new AttackState(this, attackCollider);
        defendState = new DefendState(this, shieldCollider);
        hurtState = new HurtState(this);
        deadState = new DeadState(this);
        stateMachine.ChangeState(idleState);
    }
    // VERB METHODS
    public void PlayerMovment(float intensity =1)
    {
        Vector3 movmentVector = InputToV3();
        movmentVector = movmentVector.normalized;
        movmentVector = GetCameraForward()* movmentVector ;
        movmentVector = Vector3.ProjectOnPlane(movmentVector, slopeNormal);
        Debug.DrawRay(transform.position,movmentVector*2,Color.red,.01f);
        movmentVector *= GetMovmentSpeed()*intensity;
        myRigidbody?.AddForce(movmentVector,ForceMode.Force);
    }

    public void Jump()
    {
        if(!grounded)  return;
        Debug.Log("jumped");
        animator.SetTrigger("tJump");
        myRigidbody.AddForce(Vector3.up *jumpPower, ForceMode.Impulse);
        
    }

    public bool Chop()
    {    

        if(stateMachine.currentState!=attackState)
        {
            stateMachine.ChangeState(attackState);
        } 
        return true;   
    }

    public void LearnBombSkill()
    {
        gotBombSkill = true;
    }
    public void PutBomb()
    {
        if(Input.GetKeyDown(KeyCode.Q)&&canBomb)
        StartCoroutine(PlacingBombRoutine());
        

    }
    IEnumerator PlacingBombRoutine()
    {
        canBomb =false;
        Instantiate(bombPrefab,transform.position+2*(transform.forward),Quaternion.identity);
        yield return new WaitForSeconds(bombIntervals);
        canBomb = true;
        
    }

    public int GetCurrentHealth()
    {
        Debug.Log("Current health:"+ health.GetCurrentHealth());
        return health.GetCurrentHealth();
    }

    public void Die()
    {
        if(stateMachine.currentState!= deadState)
            stateMachine.ChangeState(deadState);
    }
    private void SufferGravity()
    {
        if(forceNormalGravity) gravity = Physics.gravity;
        else gravity = DetectOnSlope() ? slopeNormal * Physics.gravity.y : Physics.gravity;
        Debug.DrawRay(transform.position, gravity.normalized * 2, Color.blue, .1f);
        myRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }

    //REACTION METHODS

    public void onTakeDamage(GameObject attacker, int damage)
    {
        Debug.Log("Simple damage feedback:Took "+damage+". Caused by: "+attacker.gameObject.name);
        stateMachine.ChangeState(hurtState);
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
        if(!isGrounded()) return;
        if(Mathf.Abs(myRigidbody.velocity.magnitude)<.5f) return;
        myRigidbody.AddForce(-myRigidbody.velocity.normalized * stopPower);
    }
    private void LimitMovmentSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
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

    public bool ReadSpaceBarInput()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    public bool ReadLeftMouseInput()
    {
        if(Input.GetMouseButton(0)&& canttrigger)
        {
            StartCoroutine("attackCD");
            return true;
        }
        return false;
    }

    public bool ReadRightMouseInput()
    {
        if(Input.GetMouseButtonDown(1)) return true;
        return false;
    }
    
   public IEnumerable attackCD()
   {
    yield return new WaitForSeconds(inputCooldown);
    canttrigger = true;
   }
    private  Vector2 ReadMovmentInput()
    {
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        float inputZ = isUp ? 1 : isDown ? -1 : 0;
        float inputX = isRight ? 1 : isLeft ? -1 : 0;
        return (new Vector2(inputX,inputZ));
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
            Debug.Log("anim triggered at stage: " + attackstage);
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

    public void TryTriggerHandAttack(Collider other, bool leftHand = false)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Attackable")) return;
        if(other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody otherRB))
        {
            var positionDiff = other.gameObject.transform.position - transform.position;
            positionDiff.Normalize();
            if(!leftHand)
            {
                 otherRB.AddForce(positionDiff*knockbackPower,ForceMode.Impulse);
                if( other.gameObject.TryGetComponent<Health>(out Health healthCom))
                {
                    healthCom.TakeDamage(this.gameObject,attackPower);
                }
            }
            else
                otherRB.AddForce(positionDiff*shieldKnocBack,ForceMode.Impulse);

        }

    }

    public float[] GetAttackDuration()
    {
        return attackDuration;
    }
    public float[] getAttackChainWindow()
    {
        return attackGracePeriod;
    }

    public Vector3 InputToV3()
    {
        return new Vector3(inputMovmentVector.x,0, inputMovmentVector.y);
    }

    public bool isGrounded()
    {
        Vector3 direction = Vector3.down;
        Bounds bounds = thisCollider.bounds;
        Vector3 origin = transform.position + new Vector3(0,bounds.size.y,0);
        float radius = bounds.size.x * 0.33f;
        float maxDistance = bounds.size.y;
        Vector3 spherePosition = direction* maxDistance + origin;
        LayerMask groundLayer = GameManager.Instance.GetGroundLayer();
        if(Physics.SphereCast (origin,radius,direction,out var hitInfo,maxDistance,groundLayer))
        {
                return true;      
        }
        animator.SetBool("bOnAir", true);
        return false;
    }

    public bool DetectOnSlope()
    {
        //todo swap ray direction to forward from the 
        //feet to calculate the angle and predict 
        //slopes so less resistance to start stairs.
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
