using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int power = 1;
    [SerializeField] float explosionRadius = 5f;
   [SerializeField] float pushPower =35;
   [SerializeField] LayerMask damageLayer;
   [SerializeField] string playerTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        Explode();
    }
    public void Explode()
    {
         Collider[] colliders= Physics.OverlapSphere(transform.position,explosionRadius,damageLayer);
         for(int colliderIndex = 0; colliderIndex<colliders.Length;colliderIndex++)
         {
            if(colliders[colliderIndex].CompareTag(playerTag))
            {
                PlayerAvatar player ;
                if(colliders[colliderIndex].gameObject.TryGetComponent(out player))
                {
                    player.onPlayerTakeDamage.Invoke(gameObject,power);
                    if(pushPower>0)
                    {
                        Vector3 pushDirection = colliders[colliderIndex].transform.position - transform.position;
                        pushDirection.Normalize(); 
                        player.onPushed.Invoke(pushPower,new Vector3(pushDirection.x,0,pushDirection.z));
                    }
                }
            }
         }

    }
}
