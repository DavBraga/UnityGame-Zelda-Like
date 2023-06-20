using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHelper : MonoBehaviour
{
    public static bool IstargetInRange(float distance, Vector3 myPos, Vector3 targetPos)
    {
        if (Vector3.Distance(myPos, targetPos) > distance) return false;
        return true;
    }

    public static bool IsTargetOnSight(Vector3 targetPos, Vector3 myPos,float sightRange)
    {
        int nonActorsMask = LayerMask.GetMask("Actors") | LayerMask.GetMask("Default");
        Vector3 direction = (targetPos -myPos).normalized;
        if(Physics.Raycast(myPos,direction,out RaycastHit hitInfo,sightRange,nonActorsMask))
        {
            if(hitInfo.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }     
        }
        Debug.DrawRay(myPos,(direction).normalized,Color.red,3f);
        return false;
    }

    public static void LookAtMyTarget(Transform myTransform)
    {
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position;
        myTransform.LookAt(targetPos);
    }

      public static Vector3 GetTargetDirection(Vector3 myPos)
    {
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position;
        return (targetPos -myPos).normalized;
    }
}
