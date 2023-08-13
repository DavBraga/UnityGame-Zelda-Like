using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Attack", menuName = "Zelda Like/Attacks/MeleeAttacks", order = 0)]
public class Attack_SO : ScriptableObject 
{
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackDuration = 1f;

    [SerializeField] float attackCoolDown = 1f;
    [SerializeField] float warmupTime = 0f;
    [SerializeField] float damageDelay = .3f;
    [Header("Power Config")]
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackPushPower = 0f;
    [SerializeField] bool lookAtMyTarget = true;

    //todo obj pooling
    [Header("Activation Effects")]
    [SerializeField] protected GameObject activationFX;

    [Header("Hit Effects")]
    [SerializeField] protected GameObject hitFX;
    protected bool doesItGotActivationFX=false;

    protected bool doesItGotHitFX=false;
    private void OnEnable() {
        doesItGotActivationFX= (activationFX!=null);

        doesItGotHitFX= (hitFX!=null);

        attackDuration = damageDelay+ attackCoolDown;
    } 

    public string animationTag = "tAttack";

    public float AttackRadius { get => attackRadius;private set => attackRadius = value; }
    public float AttackDuration { get => damageDelay+attackCoolDown+.01f;private set => attackDuration = value; }
    public float DamageDelay { get => damageDelay;private set => damageDelay = value; }
    public int AttackDamage { get => attackDamage;private set => attackDamage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackPushPower { get => attackPushPower; set => attackPushPower = value; }
    public float WarmupTime { get => warmupTime; set => warmupTime = value; }

    public Coroutine Attack(CreatureController attackerController)
    {
        return attackerController.StartCoroutine(WaitDelayAndAttack(attackerController));
    }

    IEnumerator WaitDelayAndAttack(CreatureController attackerController)
    {
        /// warmup moved here{
        yield return new WaitForSeconds(warmupTime);
        Transform controllerTransform = PrepareAttack(attackerController);
        if(!String.IsNullOrEmpty(animationTag))
           attackerController.myAnimator.SetTrigger(animationTag);
        ///warmup moved here}
        yield return new WaitForSeconds(damageDelay);
        AttackEffect(attackerController,controllerTransform);
        yield return new WaitForSeconds(attackCoolDown);
        
    }

    public virtual void AttackEffect(CreatureController attackerController, Transform controllerTransform)
    {
        
        Vector3 targetDirection = //CreatureHelper.GetTargetDirection(controllerTransform.position);
        attackerController.transform.forward;
        
        // check for player collide hits
        RaycastHit[] hits = Physics.SphereCastAll(attackerController.transform.position, attackRadius,
            targetDirection,
           AttackRange, LayerMask.GetMask("Player"));           
        // debug only on editor
        Debug.DrawRay(controllerTransform.position,
            CreatureHelper.GetTargetDirection(controllerTransform.position),
            Color.green, 3f);

        if (hits.Length < 1) return;

        // check if any hit is a player 
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (doesItGotHitFX) Instantiate(hitFX,hitObj.transform);
                if (hitObj.TryGetComponent<PlayerController>(out PlayerController player))
                {
                    DealDamage(attackerController.gameObject, player);
                    DealPushEffect(attackerController.gameObject, player, attackPushPower, new Vector3(controllerTransform.forward.x, 0, controllerTransform.forward.z));
                }      
            }
        }
    }

    protected Transform PrepareAttack(CreatureController attackerController)
    {
        Transform controllerTransform = attackerController.gameObject.transform;
        if(lookAtMyTarget)
        CreatureHelper.LookAtMyTarget(controllerTransform);
        if (doesItGotActivationFX)
            Instantiate(activationFX, attackerController.creatureCenter.position, Quaternion.identity);
        return controllerTransform;
    }

    protected void DealPushEffect(GameObject pusher,PlayerController target, float pushPower,Vector3 direction)
    {
          target.BePushed(pusher,pushPower, direction);
    }
    protected void DealDamage(GameObject attacker, PlayerController player)
    {
        if(player.TakeDamage(attacker, AttackDamage)) Debug.Log("my target died");
    }
}
