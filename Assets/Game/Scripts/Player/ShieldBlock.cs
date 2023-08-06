using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    [Range(-1, 1)]
    [Tooltip("-1 full defense, 1 no defense")]
    [SerializeField]float shieldDefenseRange = -.25f;
    public bool DirectionCanDealDamage(GameObject attacker)
    {
        Vector3 playerDirection = transform.TransformDirection(Vector3.forward);
        Vector3 attackerDirection = (transform.position- attacker.transform.position).normalized;
        float dot = Vector3.Dot(playerDirection, attackerDirection);
        Debug.Log(dot);
        if(dot<shieldDefenseRange) return false;
        
        return true;
    }
}
