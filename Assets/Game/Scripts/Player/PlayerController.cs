using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine{ get; private set;}
    public IdleState idleState{ get; private set;}
    public WalkingState walkingState{ get; private set;}
    public DeadState deadState{get; private set;}
    public OnAirState onAirState{ get; private set;}
    public AttackState attackState{get; private set;}

    public Vector2 inputMovmentVector{ get; private set;}
    public Rigidbody myRigidbody{ get; private set;}
    public  Animator animator{ get; private set;}

    private bool grounded= true;

    float attackCooldown= 0;
    

    [SerializeField] Collider thisCollider;

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
    
    [Header("Jump and MidAir")]
    [SerializeField] float jumpPower=10f;
    [SerializeField] float airMovmentSpeedModifier =0.25f;

    [Header("Attack")]
    [SerializeField] GameObject attackCollider;
    [SerializeField] float[] attackGracePeriod;
    [SerializeField] float[] attackDuration;
    [SerializeField] float inputCooldown=.001f;
    bool canttrigger= true;



    [Header("Debug")]
    [SerializeField] string currentStateName;

    public int attackstage =0;
    
    [SerializeField] GameObject[] show;
    
    private void Start()
    {
        InitializeStateMachine();
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        thisCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        currentStateName = stateMachine.GetCurrentStateName();
        if (ReadAttackInput()) Chop();
        inputMovmentVector = ReadMovmentInput();
        CalculateVelocityRate();  
        stateMachine.Update();
        
    }
    private void FixedUpdate()
    {
        ApplyGravity();
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
        deadState = new DeadState(this);
        stateMachine.ChangeState(idleState);
    }

       private void ApplyGravity()
    {
        Vector3 gravity = DetectOnSlope() ? slopeNormal * Physics.gravity.y : Physics.gravity;
        Debug.DrawRay(transform.position, gravity.normalized * 2, Color.blue, .1f);
        myRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
 
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
        myRigidbody.AddForce(Vector3.up*jumpPower, ForceMode.Impulse);
    }

    public bool Chop()
    {    

        if(stateMachine.currentState!=attackState)
        {
            stateMachine.ChangeState(attackState);
        }
        
        return true;   
    }

    public void Die()
    {
        if(stateMachine.currentState!= deadState)
            stateMachine.ChangeState(deadState);
    }
    
    public void  RotateBodyToFace(float smoothness = .15f){

        if(inputMovmentVector.isZero()) return;

        Quaternion q1 = Quaternion.LookRotation(InputToV3(),Vector3.up); 
        Quaternion q2 = GetCameraForward();
        Quaternion toRotation = q1*q2;
        Quaternion smoothRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation,smoothness);
        myRigidbody.MoveRotation(smoothRotation);

    }

    public bool ReadJumpInput()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    public bool ReadAttackInput()
    {
        if(Input.GetMouseButton(0)&& canttrigger)
        {
            StartCoroutine("attackCD");
            return true;
        }
        return false;
    }
    
   public IEnumerable attackCD()
   {
    yield return new WaitForSeconds(attackCooldown);
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
            if(show.Length<1) return;
            if(attackStage==0 ||attackStage==4)
            {
                animator.SetTrigger("tAttack1");
                show[2].SetActive(false);
                show[1].SetActive(false);
                show[0].SetActive(true);
            }
                
            else if(attackStage==1)
            {
                animator.SetTrigger("tAttack2");
                show[2].SetActive(false);
                show[0].SetActive(false);
                show[1].SetActive(true);
            }

            else if(attackStage==2)
            {
                animator.SetTrigger("tAttack3");
                show[0].SetActive(false);
                show[1].SetActive(false);
                show[2].SetActive(true);
            }
            else if(attackStage==3) animator.SetTrigger("tAttack4");

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
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = .2f;
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
