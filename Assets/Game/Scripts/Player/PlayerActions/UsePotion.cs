using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UsePotion : MonoBehaviour
{
    bool canUse = true;
    [SerializeField] float potionCD;
    [SerializeField] InventoryComunication inventoryChannel;
    [SerializeField] ItemSO potion;
    [SerializeField] int potionPower= 3;

    [SerializeField] GameObject usageFX;
    bool usesUsageFx = false;

    public UnityAction onPotionUse;
    public UnityAction onPotionCooldownEnds;
    
    Health affectedHealth; 
    // Start is called before the first frame update
    void Awake()
    {
        affectedHealth = GetComponent<Health>();
        if(usageFX) usesUsageFx = true;
    }
    private void OnEnable() {
        GetComponent<PlayerController>().onUsePotion+=Use;
    }
    private void OnDisable() {
        GetComponent<PlayerController>().onUsePotion-=Use;
    }
    public void Use()
    {
        if (canUse)
        {
            if (inventoryChannel.RemoveItem(potion))
            {
                Debug.Log("potionUsed");
                PlayPotionEffect();
                StartCoroutine(WaitForPotionCD());
            }
            else Debug.Log("not enough potions to use");
        }
    }

    public void PlayPotionEffect()
    {
        affectedHealth.Heal(potionPower);

        if(usageFX)Instantiate(usageFX, transform);
    }

    public void IncreasePotionPower(int amount = 1)
    {
        potionPower+=amount;
    }
    IEnumerator WaitForPotionCD()
    {
        onPotionUse?.Invoke();
        canUse = false;
        yield return new WaitForSeconds(potionCD);
        canUse = true;
        onPotionCooldownEnds?.Invoke();
    }
}
