using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    [Header("Detection")]
    public float sightRange=20f;

    [Header("Melee Attack")]

    public Attack_SO baseAttack;
    // public float attackRadius= 1.5f;
    // public float attackDuration= 1f;
    // public float damageDelay = .3f;
    // public int attackDamage=1;

    [Header ("Ranged Attack")]

    public Attack_SO rangedAttack;
    public Transform projectilesOrigin;

    [Header("Roaming")]
    public float searchInterval=1f;
    public float searchRadius=7f;

    [Header("Hurt")]
    public float hurtDuration= .3f;

    [Header("Chase")]
    public float ceaseFollowThreshold =2f;
    public float chaseDuration=1f;
    public Animator myAnimator{get; private set;}
    public NavMeshAgent myNavAgent{get; private set;}
    
    public Health myHealth{get; private set;}
    public Rigidbody myRigidBody{get; private set;}

     Collider mycollider;

    // states
    protected ChaseState chaseState;

    protected CreatureRoamingState creatureRoamingState;
    protected CreatureAttackState creatureAttackState;
    protected CreatureHurtState creatureHurtState;
    protected CreatureDeadState creatureDeadState;

    public StateMachine stateMachine;

     [Header ("Debug")]
    [SerializeField] string debugCurrentState;
    public virtual void Awake() {

        if(rangedAttack==null) rangedAttack= baseAttack;
        if(!projectilesOrigin) projectilesOrigin = transform;

        myNavAgent = GetComponent<NavMeshAgent>();
        myRigidBody = GetComponentInChildren<Rigidbody>();
        mycollider = GetComponentInChildren<Collider>();
        myAnimator = GetComponentInChildren<Animator>();
        stateMachine = new StateMachine();
        // new states
        SetUpStates();
        myHealth = GetComponent<Health>();
        myHealth.onTakeDamage += onTakeDamage;
    }
    public virtual void SetUpStates()
    {
        chaseState = new ChaseState(this);
        creatureRoamingState = new CreatureRoamingState(this);
        creatureAttackState = new CreatureAttackState(this);
        creatureHurtState = new CreatureHurtState(this);
        creatureDeadState = new CreatureDeadState(this);
        chaseState.SetUpState(creatureAttackState, creatureRoamingState);
        creatureAttackState.SetUpState(chaseState);
        creatureRoamingState.SetUpState(chaseState);
        creatureHurtState.SetUpState(creatureRoamingState);

        stateMachine.ChangeState(creatureRoamingState);

    }
    // Update is called once per frame
    public virtual void Update()
    {
        stateMachine.Update();
        myAnimator.SetFloat("fSpeed", myAnimator.velocity.magnitude/ myNavAgent.speed);
        //for debug only.
        debugCurrentState = stateMachine.currentState.stateName;
    }
    public virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    public virtual void LateUpdate()
    {
        stateMachine.LateUpdate();
    }
    public virtual void Attack()
    {
        baseAttack.Attack(this);
    }

    public virtual void RangedAttack()
    {
        //TODO
        Debug.Log("ranged Attack");
        rangedAttack.Attack(this);
    }

    public virtual void onTakeDamage(GameObject attacker, int damage)
    {
        Debug.Log("Simple damage feedback:Took "+damage+". Caused by: "+attacker.gameObject.name);
        if(stateMachine.currentState!= creatureHurtState)
        {
            stateMachine.ChangeState(creatureHurtState);
            myAnimator.SetTrigger("tHurt");
        }
            
    }

    public virtual void Die()
    {
       // stateMachine.ChangeState(enemyDeadState);
        myAnimator?.SetBool("bDead", true);
        myNavAgent.isStopped=true;
        myRigidBody.velocity = Vector3.zero;

        if(mycollider)
            mycollider.enabled = false;
    }
}
