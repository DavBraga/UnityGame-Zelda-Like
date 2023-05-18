using UnityEngine;
public static class VectorExtensions{

    public static bool isZero(this Vector2 v, float sqrEpson= Vector2.kEpsilon)
    {
       return v.sqrMagnitude <= sqrEpson;

    }

    public static bool isZero(this Vector3 v, float sqrEpson= Vector3.kEpsilon)
    {
       return v.sqrMagnitude <= sqrEpson;

    }
}