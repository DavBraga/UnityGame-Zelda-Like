using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction<GameObject,int> onTakeDamage;
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] GameObject destructionParticles;
    [SerializeField]protected GameObject damageParticles;
    [SerializeField]protected float FXLifetime =1.5f;
    protected int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    virtual public bool  TakeDamage(GameObject attacker, int damage)
    {
        //vfx
        if(damageParticles) Destroy(Instantiate(damageParticles, transform.position,  damageParticles.transform.rotation),FXLifetime);

        // damage 
        currentHealth -=damage;
        onTakeDamage?.Invoke(attacker,damage);

        // death management
        if(currentHealth>0) return false;
        OnDeath();
        return true;
    }
    public void OnDeath()
    {
        Debug.Log("I died // TODO");
    }
}
