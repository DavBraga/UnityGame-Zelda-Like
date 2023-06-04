using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCreatureHelper 
{
    MeleeCreatureController  creatureController;
    Vector3 myPos = new Vector3();
    Vector3 targetPos=new Vector3(); 
    public MeleeCreatureHelper(MeleeCreatureController controller)
    {
        creatureController = controller;
    }
    public bool IsTargetOnSight()
    {
        int nonActorsMask = LayerMask.GetMask("Actors") | LayerMask.GetMask("Default");
        CachePositions();
        Vector3 direction = (targetPos -myPos).normalized;
        if(Physics.Raycast(myPos,direction,out RaycastHit hitInfo, creatureController.sightRange,nonActorsMask))
        {
            if(hitInfo.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player saw!");
                return true;
            }
            
        }
        Debug.DrawRay(myPos,(direction).normalized,Color.red,3f);
        return false;
    }
    public bool IstargetInRange(float distance)
    {
        CachePositions();
        if (Vector3.Distance(myPos, targetPos) > distance) return false;
        Debug.Log("Player is in range");
        return true;
    }
    public Vector3 GetTargetDirection()
    {
        return (targetPos -myPos).normalized;
    }

    public void CachePositions()
    {
        myPos = creatureController.transform.position;
        targetPos = GameManager.Instance.GetPlayer().transform.position;
    }
    public void LookAtMyTarget()
    {
        creatureController.transform.LookAt(GameManager.Instance.GetPlayer().transform);

    }
}
