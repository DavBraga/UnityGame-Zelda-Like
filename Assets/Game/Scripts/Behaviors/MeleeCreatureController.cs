using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCreatureController : MonoBehaviour
{
    [Header("Roaming")]
    public float searchInterval =3f;
    public float searchRadius = 5f;

    [Header("Alert")]
    public float ceaseFollowInterval =2f;

    [Header("Attack")]
    public float distanceToAttack =1f;
    public float attackRadius = 1.5f;
    public float damageDelay = 0;
    public float attackDuration =2f;
    public float attackDamage = 1f;

    [Header ("Hurt")]
    public float hurtDuration = 1f;
    
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
        InstantiateStates();
        stateMachine.ChangeState(roamingState);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
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
}
