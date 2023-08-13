using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionDelay= 3f, vfxDuration = 5f;
    [SerializeField] float blastRadius=3;
    [SerializeField] int blastPower =3;
    [SerializeField] float bombRadius=1;
    [SerializeField] float pushPower= 20;
    [SerializeField] LayerMask effectMask;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionCoroutine(explosionDelay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ExplosionCoroutine(float delay=1f)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void Explode()
    {
        Destroy(Instantiate(explosionVFX, transform.position, explosionVFX.transform.rotation),vfxDuration);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius,effectMask);
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out CreatureController creatureController))
            {
                creatureController.TakeDamage(this.gameObject,blastPower);
            }
            else
                if(collider.TryGetComponent(out Health healthComp))
                    healthComp.TakeDamage(this.gameObject, blastPower,damageType.bomb);
                     
            Vector3 pushDirection = collider.transform.position - transform.position;
            pushDirection.Normalize(); 
            collider.gameObject.GetComponent<Pushable>()?.BePushed(pushPower,pushDirection);
        }
        Destroy(gameObject);
    }
}
