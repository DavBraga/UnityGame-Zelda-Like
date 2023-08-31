using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "Zelda Like/PlayerAttack/NormalAttack", order = 0)]
public class PlayerAttack : ScriptableObject 
{
    [SerializeField]PlayerAttackStruct attackStats;

    public PlayerAttackStruct GetAttackStats()
    {
        return new PlayerAttackStruct
        {
            AttackImpulse = attackStats.AttackImpulse,
            attackPreparationTime = attackStats.attackPreparationTime,
            AttackTriggerTag = attackStats.AttackTriggerTag,
            attackDuration = attackStats.attackDuration,
            attackCooldown = attackStats.attackCooldown,
            attackPower = attackStats.attackPower,
            attackknockbackPower = attackStats.attackknockbackPower
        };
    }
}

[Serializable]
public struct PlayerAttackStruct
{
    public float AttackImpulse;
    public float attackPreparationTime;
    public string AttackTriggerTag;
    public float attackDuration;
    public float attackCooldown;
    public int attackPower;
    public float attackknockbackPower;
}
