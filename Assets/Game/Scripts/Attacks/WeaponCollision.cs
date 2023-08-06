using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponCollision : MonoBehaviour
{
    public UnityAction<Collider> onHit;
    [SerializeField]GameObject hitVfx;
    bool gotHitFX;
    private void Awake() {
        if(hitVfx) gotHitFX = true;
    }
    private void OnTriggerEnter(Collider other) {
        
        onHit?.Invoke(other);
        if(!gotHitFX) return;
        Instantiate(hitVfx,other.ClosestPointOnBounds(transform.position),Quaternion.identity);
    }

    public void SetHitVfx(GameObject vfxPrefab)
    {
        hitVfx = vfxPrefab;
    }
}
