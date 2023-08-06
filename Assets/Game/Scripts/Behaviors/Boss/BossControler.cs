using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControler : CreatureController
{
    [Header("On take damage")]
    [SerializeField]float invulTime = .3f;

    [SerializeField] Vector3 rewindSpot= Vector3.zero;
    [Header("Overpower State")]

    [SerializeField] GameObject closeQuarterEffectPrefab;
    [SerializeField] GameObject overpowerCamera;
    [SerializeField] Transform returnSpot;

    [SerializeField] OverpowerStages[] overpowerStages;

    [SerializeField] MeshRenderer meshRenderer;

    [Header("Teleport")]
    [SerializeField] GameObject teleportParticle;
    [SerializeField] GameObject myGraphics;
    SkinnedMeshRenderer myRender;

    int currentStage = 0; 

    [Header("Raging State")]
    [SerializeField]public Material ragingMaterial;

    Material normalmaterial;
    [SerializeField] float ragingMovmentSpeed = 5f;
    [SerializeField] float ragingStoppingDistance = 8f;
    [SerializeField]Attack_SO ragingMeleeAttack;
    [SerializeField]Attack_SO ragingRangedAttack;
    [SerializeField] Attack_SO chaosBarrageAttack;
    CreatureAttackState ragingMeleeAttackState;
    // normal State
    CreatureAttackState ragingRangedAttackState;
    CreatureAttackState chaosBarrageAttackState;

    CreatureWaitingState creatureWaitingState;
    public StateMachine moodStateMachine = new();
    MoodState normalMoodState;
    MoodState ragingMoodState;
    MoodState overPowerState;

    override public void Awake() {

        base.Awake();
        myRender = myGraphics.GetComponent<SkinnedMeshRenderer>();
        normalmaterial = myRender.material;
    }
    public override void SetUpStates()
    {
        base.SetUpStates();
        currentStage = 0;
        creatureWaitingState = new CreatureWaitingState("creatureWaitingState");
        // override old chase state to not fall back to roaming state
        chaseState.SetUpState(creatureAttackState, chaseState);

   

        // set up chaos Barrage AttackState
        chaosBarrageAttackState = new CreatureAttackState(this);
        chaosBarrageAttackState.SetUpState(chaosBarrageAttackState,chaosBarrageAttack);

        // normal attack if close, ranged attack if ranged
        chaseState.SetUpState(creatureAttackState, creatureRangedAttackState, creatureRangedAttackState);
        creatureHurtState.SetUpState(chaseState);
        // start at a chase state
        stateMachine.ChangeState(chaseState);
        // setting up mood States
        {
            //set up overpower State
            overPowerState = new MoodState(this, "Overpower Mood State");
            overPowerState.SetUpOnEnterEffect(EnterOverPowerEffect);
            overPowerState.SetUpOnExitEffect(LeaveOverPowerEffect);

            // set up enraged attacks
            ragingMeleeAttackState = new CreatureAttackState(this);
            ragingMeleeAttackState.SetUpState(chaseState, ragingMeleeAttack);
            ragingRangedAttackState = new CreatureAttackState(this);
            ragingRangedAttackState.SetUpState(chaseState, ragingRangedAttack);
            
             //sets up raginMoodState with no leave condition;
            ragingMoodState = new MoodState(this,"ragingMoodState");
            ragingMoodState.SetUpOnEnterEffect(EnterRageMoodEffect);

            //sets up normalmoodState with a changing condition 
            normalMoodState = new MoodState(this,"normalMoodState",StageCheckPoint, overPowerState);
            // normalMoodState.SetUpState(RagingState, health<.5)
            moodStateMachine.ChangeState(normalMoodState);
        }
        if(rewindSpot!= Vector3.zero) return;
                rewindSpot = transform.position;

        
    }

    private void EnterRageMoodEffect()
    {
        overpowerCamera.SetActive(true);
        myNavAgent.speed = ragingMovmentSpeed;
        myNavAgent.stoppingDistance = ragingStoppingDistance;
        chaseState.SetUpState(ragingMeleeAttackState, ragingRangedAttackState, ragingRangedAttackState);
        meleeRange = ragingMeleeAttack.AttackRadius;
        myRender.SetMaterials(new List<Material> { ragingMaterial} );
    }

    private void LeaveOverPowerEffect()
    {
        //myNavAgent.isStopped=false;
        overpowerCamera.SetActive(false);
        myNavAgent.enabled = true;
        closeQuarterEffectPrefab.SetActive(false);
        myAnimator.SetTrigger("tLeaveOverpower");
        myHealth.SetIgnoreDamage(false);
        myNavAgent.isStopped = false;
        stateMachine.ChangeState(chaseState);
        Debug.Log("Left over power state");
        if(currentStage>0)
        overpowerStages[currentStage - 1].light.gameObject.SetActive(false);
        else
        {
            foreach(OverpowerStages stage in overpowerStages)
            {
                stage.light.gameObject.SetActive(false);
            }
        }
        if (currentStage < overpowerStages.Length)
        {
            meshRenderer.SetMaterials(new List<Material> { overpowerStages[currentStage].material });
            overpowerStages[currentStage].light.gameObject.SetActive(true);
        }
    }

    private void EnterOverPowerEffect()
    {
        StopMoving();
        HaltMovment();
        myRender.material = normalmaterial;
        overpowerCamera.SetActive(true);
        Debug.Log("entering overpower state");
        myNavAgent.isStopped=true;
        myNavAgent.ResetPath();
        
        myAnimator.SetTrigger("tOverpower");
        
        myHealth.SetIgnoreDamage(true);
        myNavAgent.enabled = false;
        Teleport();
        overpowerStages[currentStage].bindedObject.GetComponent<Health>().SetIgnoreDamage(false);
        currentStage++;
    }

    private void Teleport()
    {
        StartCoroutine(TeleportSequence());

    }
    IEnumerator GoImune()
    {
        myHealth.SetIgnoreDamage( true);
        yield return new WaitForSeconds(invulTime);
        if(moodStateMachine.currentState!=overPowerState)
            myHealth.SetIgnoreDamage(false);
    }

    IEnumerator TeleportSequence()
    {
        mycollider.enabled = false;
        stateMachine.ChangeState(creatureWaitingState);
         
        Instantiate(teleportParticle, transform.position, Quaternion.identity);  
        yield return new WaitForSeconds(.2f);
        myGraphics.SetActive(false);
        myRigidBody.MovePosition(returnSpot.position);
        yield return new WaitForSeconds(1.5f);
        Instantiate(teleportParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        closeQuarterEffectPrefab.SetActive(true);
        yield return new WaitForSeconds(.2f);
        mycollider.enabled = true;
        myGraphics.SetActive(true);
        stateMachine.ChangeState(chaosBarrageAttackState);
        
    }
    public override void Update() {

        if(GameManager.Instance.GameState == GameState.playing)
        {
            base.Update();
            moodStateMachine.Update();
        }
        
    }
    public bool StageCheckPoint()
    {
        if(myHealth.GetCurrentHealth()<overpowerStages[currentStage].healthThreshHold)
            return true;

        return false;
    }
    public void LeaveOverpower()
    {
        if(currentStage<overpowerStages.Length)
        moodStateMachine.ChangeState(normalMoodState);
        else
        moodStateMachine.ChangeState(ragingMoodState);
    }

    public override Coroutine StartAttack(Attack_SO attack)
    {
        if(stateMachine.currentState!= chaosBarrageAttackState)
        {
            StopMoving();
            HaltMovment();
        }

        return attack.Attack(this);     
    }

    public void Rewind()
    {
        LeaveOverpower();
        stateMachine.ChangeState(creatureWaitingState);
        overpowerCamera.gameObject.SetActive(false);
        StopAllCoroutines();
        SetUpStates();
        stateMachine.ChangeState(creatureWaitingState);
        Debug.Log("rewinds");
        currentStage = 0;
        
        myHealth.SetIgnoreDamage(true);
        myHealth.Heal(myHealth.GetMaxHealth()); 
        myNavAgent.enabled = false;
        mycollider.enabled = false;
       // myRigidBody.MovePosition(returnSpot.position);
        transform.position = rewindSpot;
        myRender.material = normalmaterial;
        StartCoroutine(WaitAndChase());
        
        
    }

    IEnumerator WaitAndChase()
    {
        yield return new WaitForSeconds(8f);
        mycollider.enabled = true;
        myNavAgent.enabled = true;
       // stateMachine.ChangeState(chaseState);
        myHealth.SetIgnoreDamage(false);
        moodStateMachine.ChangeState(normalMoodState);
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(chaseState);
       
    }
}


[Serializable]
public struct OverpowerStages
{
    public int healthThreshHold;

    public GameObject bindedObject;

    public Light light;

    public Material material;
    
}
