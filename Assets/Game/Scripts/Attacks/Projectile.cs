using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed=1f;
    [SerializeField] float duration = 3f;
    [SerializeField] int power = 1;

    [SerializeField] bool isPersistent= false;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] PushMode pushMode;

   [SerializeField] float pushPower =50;
    bool doesHaveExplosion = false;

    private void Awake() {
        // to avoid null check during explosion
        if(explosionPrefab) doesHaveExplosion = true;
    }
    // Start is called before the first frame update
    void Start()
    {   
        CreatureHelper.LookAtMyTargetZAxis(transform);
        if(duration>0)
        StartCoroutine(Explode(duration));
    }

    public void SetUpProjectile(int power=-1,float pushPower =-1f,float speed=-1, float duration=-1)
    {
        if(speed>=0)
        this.speed = speed;
        if(duration>=0)
        this.duration = duration;
        if(power>=0)
        this.power = power;
        if(pushPower>=0)
        this.pushPower = pushPower;

    }
    

    private void OnTriggerEnter(Collider other) {

        if(!isPersistent) Explode();
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("trriggered");
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(!player) return; 
            
            player.onPlayerTakeDamage.Invoke(this.gameObject, power);

            if(pushMode == PushMode.shoot)
            player.onPushed.Invoke(pushPower,new Vector3(transform.forward.x,0,transform.forward.z));
            else
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.Normalize(); 
                player.onPushed.Invoke(pushPower,new Vector3(pushDirection.x,0,pushDirection.z));
            }                    
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(speed>0)
        {transform.Translate(transform.forward*Time.deltaTime*speed,Space.World);
        Debug.DrawRay(transform.position, transform.forward*3);}
        
    }

    public void Explode()
    {
        if(doesHaveExplosion)
        {
            var obj = Instantiate(explosionPrefab,transform.position, Quaternion.identity);
            obj.transform.localScale *= 55;
        }
           
        Destroy(this.gameObject);
    }
    
    IEnumerator Explode(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        Explode();
    }
    public enum PushMode
    {
        shoot,
        repulsion
    }
}
