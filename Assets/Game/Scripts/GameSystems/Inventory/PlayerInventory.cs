using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    //public UnityAction<>
    [SerializeField] ItemTypeSO keys;
    [SerializeField] ItemTypeSO potions;
    [SerializeField] ItemTypeSO bossKeys;
    [SerializeField]int ownedKeys=0;
    [SerializeField]int ownedPotions=0;
    [SerializeField] int maxOwnedPotions=0;
    [SerializeField] InventoryComunication inventoryComunicationChannel;
    bool hasBossKey = false;
    private void OnEnable() {
        if(inventoryComunicationChannel)
        inventoryComunicationChannel.RegisterInventory(this);
    }
    private void OnDisable() {
        if(inventoryComunicationChannel)
        inventoryComunicationChannel.UnregisterInventory(this);
    }
    public void AddItem(ItemSO item)
    {
        if(item.GetItemType() == keys)
        {
            AddKey();
        }
        else if(item.GetItemType()==bossKeys)
        {
            hasBossKey = true;
        }
        else if(item.GetItemType()== potions)
        {
            AddPotion();
        }
    }
    public bool RemoveItem(ItemSO item)
    {
        if(item.GetItemType() == keys)
        {
          return RemoveKey();
        }
        else if(item.GetItemType()==bossKeys)
        {
            hasBossKey =false;
        }
        else if(item.GetItemType()== potions)
        {
            RemovePotion();
        }
        return true;
    }
    private void AddKey()
    {
        ownedKeys ++;
    }
    private bool RemoveKey()
    {
        if(--ownedKeys<0)
        {
            ownedKeys = 0;
            return false;
        }
        return true;
    }

    public int GetKeyCount()
    {
        return ownedKeys;
    }

    private void AddPotion()
    {
        if(++ownedPotions>maxOwnedPotions) 
            ownedPotions = maxOwnedPotions;
    }
     private bool RemovePotion()
    {
        if(--ownedPotions<0)
        {
            ownedPotions =0;
            return false;
        } 
        return true;
    }
}
