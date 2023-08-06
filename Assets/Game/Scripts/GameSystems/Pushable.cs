using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pushable : MonoBehaviour
{
    Rigidbody myrigidBody;
    Bounds bounds;
    float timeOfPush;
    Coroutine rbRenabler;
    float rbOffDuration = .1f;

    boolConditionDelegate sucessCondition= ()=>{return true;};

    bool isKinematic = false;
    private void Awake() {
        myrigidBody = GetComponent<Rigidbody>();
        bounds = GetComponent<Collider>().bounds;
        isKinematic = myrigidBody.isKinematic;
    }

    public void SetASuccessConditoon(boolConditionDelegate conditon)
    {
        sucessCondition = conditon;
    }
    public void BePushed(float pushPower, Vector3 direction)
    {
        if(!sucessCondition()) return ;
        if(Time.time<timeOfPush+.3f) return;
        timeOfPush = Time.time;
        float maxDistance= bounds.size.y+.1f;
        bool isTouchingSomethingAhead = Physics.Raycast(transform.position,direction*maxDistance,GameManager.Instance.GetCollisionLayer());
        if(isTouchingSomethingAhead) return;


        if(!isKinematic)myrigidBody.AddForce(pushPower*direction.normalized, ForceMode.Impulse);
        else
       { 
            myrigidBody.isKinematic = false;
            myrigidBody.AddForce(pushPower*direction, ForceMode.Impulse);
            StopAllCoroutines();
            rbRenabler = StartCoroutine(RenableRB());
        }
            
    }
    IEnumerator RenableRB()
    {
        yield return new WaitForSeconds(rbOffDuration);
        myrigidBody.isKinematic = true;
    }
}
