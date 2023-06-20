using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_RangedAttack", menuName = "Zelda Like/Attacks/Ranged Attacks", order = 0)]
public class RangedAttack_SO : Attack_SO
{
    [Header ("Projectile")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] float projectileLifeTime=1f;

    public override void Attack(CreatureController controller)
    {
        //base.Attack(controller);
        Debug.Log("Used an ranged attack");
        Instantiate(projectilePrefab,controller.projectilesOrigin.position,controller.transform.rotation);
    }
}
