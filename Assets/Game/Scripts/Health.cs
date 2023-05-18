using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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

    virtual public bool  OnTakeDamage(int damage)
    {
        if(damageParticles) Destroy(Instantiate(damageParticles, transform.position,  damageParticles.transform.rotation),FXLifetime);
        currentHealth -=damage;
        if(currentHealth>0) return false;

        OnDeath();
        return true;

    }

    public void OnDeath()
    {
        if(destructionParticles) 
            Destroy(Instantiate(destructionParticles, transform.position,  destructionParticles.transform.rotation),FXLifetime);
        Destroy(gameObject);
    }
}
