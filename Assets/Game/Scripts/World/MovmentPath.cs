using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentPath : MonoBehaviour
{
    public Transform GetWayPoint(int wayPointIndex)
    {
        return transform.GetChild(wayPointIndex);
    }
    public int GetNextWayPointIndex(int currentWayPointIndex)
    {
        int nextWayPointIndex = currentWayPointIndex+1;
        if(nextWayPointIndex>= transform.childCount)
        {
            nextWayPointIndex = 0;
        }

        return nextWayPointIndex;
    }
}
