using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MeleeCreatureController : MonoBehaviour
{
    public UnityAction hurtCallBack;
    public Health health{get; private set;}
    [Header("Detection")]
    public float sightRange= 5f;
    [Header("Roaming")]
    public float searchInterval =3f;
    public float searchRadius = 5f;

    [Header("Alert")]
    public float ceaseFollowThreshold =2f;
    public float followDistance = 10f;
    public float followInterval = 2f;

    [Header("Attack")]
    public float distanceToAttack =1f;
    public float attackRadius = 1.5f;
    public float damageDelay = .3f;
    public float attackDuration =2f;
    public int attackDamage = 1;
    

    [Header ("Hurt")]
    public float hurtDuration = 1f;

    public NavMeshAgent myNavAgent{get; private set;}

    [Header ("Debug")]
    [SerializeField] string debugCurrentState;
    
    //STATE MACHINE
    public MeleeCreatureHelper helper{get; private set;}
    public StateMachine stateMachine{get; private set;}
    public RoamingState roamingState{get; private set;}
    public AlertState alertState{get; private set;}
    public EnemyAttackState enemyAttackState{get; private set;}
    public EnemyHurtState enemyHurtState{get; private set;}
    public EnemyDeadState enemyDeadState{get; private set;}
    private void Awake() {
        stateMachine = new StateMachine();
        helper = new MeleeCreatureHelper(this);
        myNavAgent = GetComponent<NavMeshAgent>();
        InstantiateStates();
        stateMachine.ChangeState(roamingState);

       health = GetComponent<Health>();
       health.onTakeDamage += onTakeDamage;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        // for debug only.
        debugCurrentState = stateMachine.currentState.stateName;
    }
    private void FixedUpdate() 
    {
        stateMachine.FixedUpdate();    
    }

    private void LateUpdate() {
        stateMachine.LateUpdate();
    }
    public void InstantiateStates()
    {
        roamingState = new RoamingState(this);
        alertState = new AlertState(this);
        enemyAttackState = new EnemyAttackState(this);
        enemyHurtState = new EnemyHurtState(this);
        enemyDeadState = new EnemyDeadState(this);
    }

    public void Attack()
    {
        helper.CachePositions();
        helper.LookAtMyTarget();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,1f,helper.GetTargetDirection(),attackRadius*5,LayerMask.GetMask("Actors"));
        Debug.DrawRay(transform.position, helper.GetTargetDirection(),Color.green, 3f);
        if(hits.Length>0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    GameObject hitObj =hit.collider.gameObject;
                    if(hitObj.TryGetComponent<Health>(out Health health))
                    {
                        health.TakeDamage(gameObject, attackDamage);
                    }
                }    
            }
        }

    }
    public void onTakeDamage(GameObject attacker, int damage)
    {
        Debug.Log("Simple damage feedback:Took "+damage+". Caused by: "+attacker.gameObject.name);
        if(stateMachine.currentState!= enemyHurtState)
            stateMachine.ChangeState(enemyHurtState);
    }

    public void Die()
    {
        stateMachine.ChangeState(enemyDeadState);
        myNavAgent.isStopped=true;
    }
}
