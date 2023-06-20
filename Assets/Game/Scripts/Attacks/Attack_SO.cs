using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Attack", menuName = "Zelda Like/Attacks/MeleeAttacks", order = 0)]
public class Attack_SO : ScriptableObject 
{
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float damageDelay = .3f;
    [SerializeField] int attackDamage = 1;

    public string animationTag = "tAttack";

    public float AttackRadius { get => attackRadius;private set => attackRadius = value; }
    public float AttackDuration { get => attackDuration;private set => attackDuration = value; }
    public float DamageDelay { get => damageDelay;private set => damageDelay = value; }
    public int AttackDamage { get => attackDamage;private set => attackDamage = value; }

    public virtual void Attack(CreatureController controller)
    {
        Transform controllerTransform = controller.gameObject.transform;
        CreatureHelper.LookAtMyTarget(controllerTransform);

        // check for player collide hits
        RaycastHit[] hits = Physics.SphereCastAll(controller.transform.position,1f,
            CreatureHelper.GetTargetDirection(controllerTransform.position),
            AttackRadius*5,LayerMask.GetMask("Actors"));
        
        // debug only on editor
        Debug.DrawRay(controllerTransform.position, 
            CreatureHelper.GetTargetDirection(controllerTransform.position),
            Color.green, 3f);
        

        // check if any hit is a player 
        if(hits.Length>0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    GameObject hitObj =hit.collider.gameObject;
                    if(hitObj.TryGetComponent<Health>(out Health health))
                    {
                        health.TakeDamage(controller.gameObject,AttackDamage);
                    }
                }    
            }
        }

    }


}
