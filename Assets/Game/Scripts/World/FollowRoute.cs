using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRoute : MonoBehaviour
{
    [SerializeField] bool isMoving = true;

    [SerializeField] MovmentPath path;

    [SerializeField] float travelSpeed;

    int targetWayPointIndex;
    private Transform previousWaypoint;
    private Transform targetWayPoint;

    private float timeToWayPoint;
    private float elapsedTime;

    private void Start() {
        TargetNextWayPoint();
    }
    private void TargetNextWayPoint()
    {

        previousWaypoint = path.GetWayPoint(targetWayPointIndex);
        targetWayPointIndex = path.GetNextWayPointIndex(targetWayPointIndex);
        targetWayPoint = path.GetWayPoint(targetWayPointIndex);
        elapsedTime = 0;

        float distance= Vector3.Distance(previousWaypoint.position, targetWayPoint.position);
        timeToWayPoint = distance/travelSpeed;
    }

    void FixedUpdate()
    {
        if(!isMoving) return;

        
        elapsedTime += Time.deltaTime;
        float elapsedPercent = elapsedTime/ timeToWayPoint;
        elapsedPercent = Mathf.SmoothStep(0,1, elapsedPercent);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWayPoint.position, elapsedPercent);

        if(elapsedPercent>=1) TargetNextWayPoint();
    }
}
