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
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider collider in colliders)
        {
            Debug.Log("bomb hits "+colliders.Length+" targets.");
            if(collider.TryGetComponent<Health>(out Health healthComp))
            {
                Debug.Log("got a health componenet target");
                // damage for ship
                if(collider.gameObject.CompareTag("enemy"))
                {
                    //healthComp.OnTakeDamage(1);
                    healthComp.TakeDamage(this.gameObject, 1);
                    continue;
                } 
                // damage for planks
                // float dis = Vector3.Distance(transform.position, collider.gameObject.transform.position)-bombRadius*2;
                // float disRate = dis/(blastRadius);
                // disRate = Mathf.Clamp01(disRate);
                // float damage  = blastPower - blastPower*disRate;
                // healthComp.OnTakeDamage(Mathf.FloorToInt(damage));
            }
            
        }
        Destroy(gameObject);
    }
}
