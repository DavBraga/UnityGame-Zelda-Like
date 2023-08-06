using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_SummonSpell", menuName = "Zelda Like/Attacks/Summon Spells", order = 0)]
public class SummonSpell : Attack_SO
{
    [SerializeField] GameObject creaturePrefab;
    public override void AttackEffect(CreatureController controller, Transform controllerTransform)
    {
        //todo use pooling instead
       GameObject creature = Instantiate(creaturePrefab,controller.projectilesOrigin.position,controller.projectilesOrigin.rotation);
    }

}
