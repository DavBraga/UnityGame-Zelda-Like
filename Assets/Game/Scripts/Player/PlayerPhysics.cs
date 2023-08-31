using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Rigidbody myRigidbody;
    private PlayerAvatar playerAvatar;
    PlayerController playerController;
    bool forceNormalGravity;
    Pushable pushable;

    public Collider thisCollider;

    [Header("Movment")]
    [SerializeField] float acceleration = 10f;
    [SerializeField] float maxSpeed = 10f;

    [Header("Jump and MidAir")]
    [SerializeField] float jumpPower=10f;
    [SerializeField] float airMovmentSpeedModifier =0.25f;

    float fVelocityRate;

    [Header("Slope")]
    [SerializeField]float slopeAngle=0;
    [SerializeField]float  MaxSlopeAngle= 45;

    [Header("Snap Stop")]
    [SerializeField] bool SnapStop = false;
    [Tooltip("Needs Snap Stop = true to have any effect.")]
    [SerializeField] float ExtraStopForce=5f;
    private Vector3 slopeNormal;
    Vector3 gravity;
    Bounds bounds;

    // movement, gravity and jump

    // For further refactor, check animator prompts.

    private void Awake() {
        playerAvatar = GetComponent<PlayerAvatar>();
        myRigidbody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
        pushable = GetComponent<Pushable>();
        playerController = playerAvatar.GetPlayerController();
    }
    private void Start() {
        bounds = thisCollider.bounds;
    }

   private void OnEnable() {
        playerAvatar.onJump+= Jump;
        playerAvatar.onMove+= PlayerManagedMove;
        playerAvatar.onRotate+= RotateBodyToFace;
        playerController.onMovmentInput+=CalculateVelocityRate;
        playerAvatar.isGroundedDelegate+=IsGrounded;
        playerAvatar.onAir += ForceNormalGravity;
        playerAvatar.onLand+= StopForcingNormalGravity;
        playerAvatar.onPlayerImpulse+=PlayerImpulse;
        playerAvatar.onPushed +=BePushed;
        playerAvatar.onStateInitializationFinished+=SetUpAirState;
        playerAvatar.onDeath+= TurnOffPhysics;
        playerAvatar.onRessurect+=TurnOnPhysics;
       // playerController.onControlRecover += TurnOnPhysics;
 
   }
   private void OnDisable() {
        playerAvatar.onJump-= Jump;
        playerAvatar.onMove-= PlayerManagedMove;
        playerAvatar.onRotate-= RotateBodyToFace;
        playerController.onMovmentInput-=CalculateVelocityRate;
        playerAvatar.isGroundedDelegate-=IsGrounded;
        playerAvatar.onAir -= ForceNormalGravity;
        playerAvatar.onLand -= StopForcingNormalGravity;
        playerAvatar.onPlayerImpulse-=PlayerImpulse;
        playerAvatar.onPushed -=BePushed;
        playerAvatar.onStateInitializationFinished-=SetUpAirState;
        playerAvatar.onDeath-= TurnOffPhysics;
        playerAvatar.onRessurect-=TurnOnPhysics;
        //playerController.onControlRecover -= TurnOnPhysics;
   }

   public void SetUpAirState()
   {
        playerController.OnAirState.SetAirMovmentModifier(airMovmentSpeedModifier);
   }


    public void PlayerMove( float intensity =1)
    {
        Vector3 movmentVector = playerController.InputToV3();
        movmentVector = movmentVector.normalized;
        movmentVector = playerAvatar.GetCameraForward()* movmentVector ;
        if(!forceNormalGravity)
        movmentVector = Vector3.ProjectOnPlane(movmentVector, slopeNormal);
        float maxDistance= bounds.size.z;//+.1f;
        Vector3 castOrigin =transform.position- new Vector3(0,0,bounds.size.z*.5f);

        bool isTouchingSomethingAhead ;
        isTouchingSomethingAhead = Physics.SphereCast(castOrigin,bounds.size.z*.4f,movmentVector,out _, maxDistance,GameManager.Instance.GetCollisionLayer())||
        Physics.SphereCast(castOrigin+ new Vector3(0,bounds.size.y,bounds.size.z*.5f),bounds.size.z*.4f,movmentVector,out _, maxDistance,GameManager.Instance.GetCollisionLayer());

             if(!IsGrounded()&&isTouchingSomethingAhead)
             {
                return;
             } 
        Debug.DrawRay(castOrigin,movmentVector*2,Color.red,.01f);
        movmentVector *= acceleration*intensity;
        myRigidbody?.AddForce(movmentVector,ForceMode.Force);
    }

    public void PlayerManagedMove(float intensity =1)
    {
        PlayerMove(1);
        LimitMovmentSpeed(intensity);
    }

   


    public void  RotateBodyToFace(float smoothness = .15f){

        if(playerController.inputMovmentVector.isZero()) return;

        Quaternion q1 = Quaternion.LookRotation(playerController.InputToV3(),Vector3.up); 
        Quaternion q2 = playerAvatar.GetCameraForward();
        Quaternion toRotation = q1*q2;
        Quaternion smoothRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation,smoothness);
        myRigidbody.MoveRotation(smoothRotation);

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
            playerAvatar.Animator.SetBool("bOnAir", false);
                return true;      
        }
        playerAvatar.Animator.SetBool("bOnAir", true);
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

      private void CalculateVelocityRate()
    {
        fVelocityRate = 0;

            fVelocityRate = myRigidbody.velocity.magnitude/maxSpeed;
            playerAvatar.Animator.SetFloat("fVelocity", fVelocityRate);
    } 

     private void SufferGravity()
    {
        if(forceNormalGravity) gravity = Physics.gravity;
        else gravity = DetectOnSlope() ? slopeNormal * Physics.gravity.y : Physics.gravity;
        Debug.DrawRay(transform.position, gravity.normalized * 2, Color.blue, .1f);
        myRigidbody.AddForce(gravity, ForceMode.Acceleration);
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

    public void Jump()
    {
        Debug.Log("try jump");
        if(!IsGrounded())  return;
        playerAvatar.Animator.SetTrigger("tJump");
        Debug.Log("Jumps");
        myRigidbody.AddForce((Vector3.up *jumpPower)-gravity.normalized, ForceMode.Impulse);
    }

    private void StopFaster(float stopPower=1f)
    {
        if(!IsGrounded()) return;
        if(Mathf.Abs(myRigidbody.velocity.magnitude)<.5f) return;
        myRigidbody.AddForce(-myRigidbody.velocity.normalized * stopPower);
    }

    private void FixedUpdate() 
    {
        SufferGravity();
        if(GameManager.Instance.GameState != GameState.playing) return;

        if(!SnapStop) return;
        if(playerAvatar.GetPlayerController().inputMovmentVector.isZero())StopFaster(ExtraStopForce);
    }
    private void Update() {
        CalculateVelocityRate();
    }

    public void PlayerImpulse(float impulesePower)
    {
        myRigidbody.AddForce(transform.forward*impulesePower ,ForceMode.Impulse);
    }

    public void BePushed(float pushPower, Vector3 direction)
    {
        pushable.BePushed(pushPower, direction);
    }
    public void TurnOffPhysics()
    {
        thisCollider.enabled = false;
        myRigidbody.isKinematic = true;
    }
    public void TurnOnPhysics()
    {
        thisCollider.enabled = true;
        myRigidbody.isKinematic = false;
    }

    public void ForceNormalGravity()
    {
        forceNormalGravity = true;
    }
    public void StopForcingNormalGravity()
    {
        forceNormalGravity = false;

    }
}
