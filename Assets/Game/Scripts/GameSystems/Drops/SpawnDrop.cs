using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDrop : MonoBehaviour
{
    [SerializeField] Drop[] drop;
    // Start is called before the first frame update

    public void FireDrop()
    {
        foreach(Drop entry in drop)
        {
            float randomValue = UnityEngine.Random.Range(0f,1f);
            Debug.Log("randomValue :"+randomValue+" Droprate: "+ entry.droprate);
            if(randomValue<entry.droprate)
                Instantiate(entry.dropPrefab,transform.position,Quaternion.identity);
        }
    }
    [Serializable]
    struct Drop
    {
      public GameObject dropPrefab;
       public float droprate;
        
    }
}
