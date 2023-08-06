using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_RangedAttack", menuName = "Zelda Like/Attacks/Ranged Attacks", order = 0)]
public class RangedAttack_SO : Attack_SO
{
    [Header ("Projectile")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] float projectileLifeTime=1f;
    [SerializeField] float projectileTravelSpeed = 10f;
    bool isDirectProjectile = false;
    private void Awake() {
        if(projectilePrefab.TryGetComponent<Projectile>(out Projectile projectile))
        {
            isDirectProjectile = true;
        }
    }

    public override void AttackEffect(CreatureController controller, Transform controllerTransform)
    {
        //todo use pooling instead
       GameObject projectile = Instantiate(projectilePrefab,controller.projectilesOrigin.position,controller.projectilesOrigin.rotation);
       if(isDirectProjectile)
       projectile.GetComponent<Projectile>().SetUpProjectile(AttackDamage, AttackPushPower,projectileTravelSpeed,projectileLifeTime);
    }
}
