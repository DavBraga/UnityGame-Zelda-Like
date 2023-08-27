using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class WeaponCollision : MonoBehaviour
{
    public UnityAction<Collider> onHit;
    [SerializeField]int maxParticleCount = 5;
    int particlePointer= 0;
    List<GameObject> particleList = new();
    [SerializeField]GameObject hitVfx;
    bool instantiated = false;
    GameObject instantiatedVFX;
    [SerializeField] LayerMask layer;
    Collider colliderComponent;
    bool gotHitFX;
    private void Awake()
    {
        if (hitVfx) gotHitFX = true;
        colliderComponent = GetComponent<Collider>();
        InstantiateVFXs();
    }

    private void InstantiateVFXs()
    {
        if (hitVfx)
            for (particlePointer = 0; particlePointer < maxParticleCount; particlePointer++)
            {
                particleList.Add(Instantiate(hitVfx));
            }
        particlePointer = 0;
    }
    private void OnTriggerEnter(Collider other) {
        
        onHit?.Invoke(other);

        if(particleList.Count>0) 
        {  
            particleList[particlePointer].transform.SetPositionAndRotation(other.ClosestPointOnBounds(transform.position), quaternion.identity);
            particleList[particlePointer].SetActive(true);
            particlePointer++;
            if(particlePointer>=maxParticleCount) particlePointer = 0;
        }
        
    }

    public void SetHitVfx(GameObject vfxPrefab)
    {
        hitVfx = vfxPrefab;
    }
}
