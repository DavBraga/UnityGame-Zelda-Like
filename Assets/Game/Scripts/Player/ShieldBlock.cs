using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    [SerializeField] GameObject shieldblockVFX;
    GameObject instantiatedVFX;
    bool instantiated = false;
    [Range(-1, 1)]
    [Tooltip("-1 full defense, 1 no defense")]
    [SerializeField]float shieldDefenseRange = -.25f;
    public bool DirectionCanDealDamage(GameObject attacker)
    {
        Vector3 playerDirection = transform.TransformDirection(Vector3.forward);
        Vector3 attackerDirection = (transform.position- attacker.transform.position).normalized;
        float dot = Vector3.Dot(playerDirection, attackerDirection);
        Debug.Log(dot);
        if(dot<shieldDefenseRange)
        {
            if(!instantiated)
            {
                instantiatedVFX = Instantiate(shieldblockVFX,transform.position,Quaternion.identity);
                instantiated = true;
            }
            else
            {
                instantiatedVFX.transform.SetLocalPositionAndRotation(transform.position,quaternion.identity);
                instantiatedVFX.SetActive(true);
            }
            return false;
        } 
        
        return true;
    }
}
