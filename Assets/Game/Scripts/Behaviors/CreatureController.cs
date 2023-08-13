using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    Vector3 startingPostion = new();
    public Transform creatureCenter;
    [Header("Detection")]
    public float sightRange=20f;
    public float meleeRange = 1.5f;
    public float hearRange = 5f;

    [Header("Melee Attack")]

    public bool isattacking= false;
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
    public float staggerImunityDuration=0.15f;
    bool isStaggerImune = false;
    float timeOfDamage;

    float staggeerMoment=0f;
    [SerializeField]bool isUnstoppable = false;

    [Header("Chase")]
    public float ceaseFollowThreshold =2f;
    public float rangedStateIntervals = 0;
    public bool rangedOverMelee = false;
    public Animator myAnimator{get; private set;}
    public NavMeshAgent myNavAgent{get; private set;}

    [Header("Despawn")]
    [SerializeField]bool doesItDespawns = true;
    [SerializeField] GameObject despawnParticlePrefab;
    [SerializeField] float despawnDelay = 1f;

     Quaternion startingRotation;
    
    public Health myHealth{get; private set;}
    public Rigidbody myRigidBody{get; private set;}

    protected Collider mycollider;

    // states
    protected ChaseState chaseState;

    protected CreatureRoamingState creatureRoamingState;
    protected CreatureAttackState creatureAttackState;

    protected CreatureAttackState creatureRangedAttackState;
    protected CreatureHurtState creatureHurtState;
    protected CreatureDeadState creatureDeadState;
    public StateMachine stateMachine;

     [Header ("Debug")]
    [SerializeField] string debugCurrentState;
    public virtual void Awake()
    {
        if(creatureCenter==null) creatureCenter = transform;

        //if (rangedAttack == null) rangedAttack = baseAttack;
        
        if (!projectilesOrigin) projectilesOrigin = transform;
        SetUpComponenetsReferences();

        stateMachine = new StateMachine();
        SetUpStates();
    }
    IEnumerator WaitForGameManager()
    {
        Debug.Log("wait for manager");
        yield return new WaitUntil(()=> GameManager.IsManagerReady());
        Debug.Log("Manager ready");
        yield return new WaitUntil(()=>GameManager.Instance.CheckForPlayer());
        Debug.Log("found player");
        GameManager.Instance.GetPlayer().GetComponent<PlayerController>().onDeath+= ReturnHome;
    }

    private void OnDisable() {
        if(GameManager.Instance.GetPlayer())
        GameManager.Instance.GetPlayer().GetComponent<PlayerController>().onDeath-= ReturnHome;
    }
    private void Start() {
        startingPostion = transform.position;
        startingRotation = transform.rotation;
        StartCoroutine(WaitForGameManager());
    }

    private void ReturnHome()
    {
        Debug.Log("returning home");
        stateMachine.ChangeState(creatureRoamingState);
        StartCoroutine(WaitAndReturn());
        
        
    }

    IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(3.5f);
        myNavAgent.ResetPath();
        transform.position = startingPostion;
        transform.rotation = startingRotation;

    }

    private void SetUpComponenetsReferences()
    {
        myNavAgent = GetComponent<NavMeshAgent>();
        myRigidBody = GetComponent<Rigidbody>();
        mycollider = GetComponent<Collider>();
        myAnimator = GetComponentInChildren<Animator>();
        myHealth = GetComponent<Health>();
    }

    public virtual void SetUpStates()
    {
        chaseState = new ChaseState(this);
        creatureRoamingState = new CreatureRoamingState(this);
        creatureAttackState = new CreatureAttackState(this);
        
        if(rangedAttack)
        {
            // set up a ranged attack state
            creatureRangedAttackState = new CreatureAttackState(this);
            creatureRangedAttackState.SetUpState(chaseState, rangedAttack);
            chaseState.SetUpState(creatureAttackState, creatureRoamingState,creatureRangedAttackState);
        }
        else
            chaseState.SetUpState(creatureAttackState, creatureRoamingState);

        creatureHurtState = new CreatureHurtState(this);
        creatureDeadState = new CreatureDeadState(this);
        
        creatureAttackState.SetUpState(chaseState, baseAttack);
        creatureRoamingState.SetUpState(chaseState);
        creatureHurtState.SetUpState(chaseState);

        stateMachine.ChangeState(creatureRoamingState);

    }
    // Update is called once per frame
    public virtual void Update()
    {
        if(GameManager.Instance.GameState == GameState.playing)
        {
            stateMachine.Update();
            myAnimator.SetFloat("fSpeed", myNavAgent.velocity.magnitude);
            //for debug only.
            debugCurrentState = stateMachine.currentState.stateName;
        }
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

    public virtual void TakeDamage(GameObject attacker, int damage)
    {
        if(Time.time<timeOfDamage+0.3f) return;

        timeOfDamage = Time.time;
        myHealth.TakeDamage(attacker,damage);
        if(myHealth.GetCurrentHealth()<1)
        {
            Die();
            return;
        }
        if(stateMachine.currentState==creatureRoamingState) 
        stateMachine.ChangeState(chaseState);
        isStaggerImune =Time.time<(staggeerMoment+staggerImunityDuration+hurtDuration);
        if(isUnstoppable||isStaggerImune) return;
        myAnimator.SetTrigger("tHurt");
        staggeerMoment = Time.time;
        
       // if(stateMachine.currentState!= creatureHurtState)
       // {
        
            stateMachine.ChangeState(creatureHurtState);
            
       // }
            
    }
    public virtual void Die()
    {
        GameManager.Instance.GetPlayer().GetComponent<PlayerController>().onDeath -= ReturnHome;
        stateMachine.ChangeState(creatureDeadState);
        myAnimator?.SetBool("bDead", true);
        myNavAgent.isStopped=true;
        myRigidBody.isKinematic = true;

        if(mycollider)
            mycollider.enabled = false;

        if(doesItDespawns)
        StartCoroutine(Despawn(despawnDelay));
    }


    IEnumerator Despawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(despawnParticlePrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public virtual Coroutine StartAttack(Attack_SO attack)
    {
        StopMoving();
        HaltMovment();
        return attack.Attack(this);     
    }

    public virtual void StopMoving()
    {
        if(myNavAgent)
        myNavAgent.isStopped = true;
        if(!myRigidBody.isKinematic)
        myRigidBody.velocity = Vector3.zero;
    }
    public virtual void HaltMovment()
    {
        myNavAgent.ResetPath();
        if(!myRigidBody.isKinematic)
        myRigidBody.velocity = Vector3.zero;
    }
}
