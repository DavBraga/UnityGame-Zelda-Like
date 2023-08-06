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

    public static bool CanIHearMyTarget(Vector3 targetPos,Transform myTransform,float hearRange, float myEyeHeight=.7f)
    {
        int nonActorsMask = LayerMask.GetMask("Actors") | LayerMask.GetMask("Default");
        // check if target is close to be heared
        if(!IstargetInRange(hearRange,myTransform.position, targetPos)) return false;
        Vector3 direction = (targetPos - myTransform.position).normalized;
        return CheckForObstaclesInPath(myTransform, direction,hearRange,nonActorsMask,myEyeHeight);
    }

    public static bool IsTargetOnSight(Vector3 targetPos,Transform myTransform,float sightRange,float dotFactor=.15f, float myEyeHeight=.7f)
    {
        int nonActorsMask = LayerMask.GetMask("Actors") | LayerMask.GetMask("Default");
        Vector3 direction = (targetPos - myTransform.position).normalized;
        
        if(Vector3.Dot(myTransform.forward,direction)<dotFactor) return false;

        return CheckForObstaclesInPath(myTransform, direction, sightRange, nonActorsMask,myEyeHeight);
    }

    public static bool CheckForObstaclesInPath(Transform myTransform,Vector3 direction,float checkRange, LayerMask nonActorsMask,float myEyeHeight=.7f)
    {
        if(Physics.Raycast(myTransform.position+ new Vector3(0,myEyeHeight,0),direction,out RaycastHit hitInfo,checkRange,nonActorsMask))
        {
            Debug.DrawRay(myTransform.position,(direction).normalized,Color.green,checkRange);
            if(hitInfo.collider.gameObject.CompareTag("Player")) return true;   
        }
        return false;
    }

    public static void LookAtMyTarget(Transform myTransform)
    {
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position;
        targetPos.y = myTransform.position.y; 
        myTransform.LookAt(targetPos);
    }

    public static void LookAtMyTargetZAxis(Transform myTransform)
    {
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position+ new Vector3(0,1.4f,0);
        Vector3 myRotation = myTransform.eulerAngles;
        myTransform.LookAt(targetPos);
        Vector3 tempRotation = myTransform.eulerAngles;
        myTransform.eulerAngles = new Vector3(tempRotation.x,myRotation.y,myRotation.z);

    }

      public static Vector3 GetTargetDirection(Vector3 myPos)
    {
        Vector3 targetPos = GameManager.Instance.GetPlayer().transform.position;
        return (targetPos -myPos).normalized;
    }
}
