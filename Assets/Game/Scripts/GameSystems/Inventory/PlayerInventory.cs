using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public UnityAction<ItemSO> onChangeItems;
    [SerializeField] InventoryItem[] acquirableItems;
    [SerializeField] InventoryComunication inventoryComunicationChannel;
    AudioSource audioSource;

    Dictionary<ItemSO,int> inventoryItens= new();
    Dictionary<ItemSO,int> inventoryItensMaxValues= new();

    private void Awake() {
        foreach(InventoryItem itemEntry in acquirableItems)
        {
            inventoryItens.Add(itemEntry.item,itemEntry.startingAmount);
            inventoryItensMaxValues.Add(itemEntry.item,itemEntry.MaxAcquirable);
        }
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable() {
        if(inventoryComunicationChannel)
        inventoryComunicationChannel.RegisterInventory(this);
    }
    private void OnDisable() {
        if(inventoryComunicationChannel)
        inventoryComunicationChannel.UnregisterInventory(this);
    }

    public bool AddItem(ItemSO item,int amount=1)
    {
        if(!inventoryItens.ContainsKey(item))
        {
            Debug.Log("Trying to acquire a non acquirable item:"+ item.name);
            return false;
        }
        if(inventoryItens[item]>=inventoryItensMaxValues[item]) return false;
        inventoryItens[item]+=amount;
        if(inventoryItens[item]>inventoryItensMaxValues[item])
            inventoryItens[item] = inventoryItensMaxValues[item];
        
        onChangeItems?.Invoke(item);
        
        if(item.GotSound())
        audioSource.PlayOneShot(item.GetSound());
        return true;
    }
    public bool RemoveItem(ItemSO item,int amount=1)
    {
        if(!inventoryItens.ContainsKey(item))
        {
            Debug.LogError("Trying to remove a non acquirable item:"+ item.name);
            return false;
        } 
        inventoryItens[item]-=amount;

        if(inventoryItens[item]<0)
        {
            inventoryItens[item] = 0;
            
            return false;
        }
        onChangeItems?.Invoke(item);
        return true; 
    }
    public int GetItemCount(ItemSO item)
    {
        if(!inventoryItens.ContainsKey(item))
        {
            Debug.LogError("Trying to get a non acquirable item:"+ item.name);
            return -1;
        } 
        return inventoryItens[item];
    }

        public int GetMaxItemCount(ItemSO item)
    {
        if(!inventoryItens.ContainsKey(item))
        {
            Debug.LogError("Trying to get a non acquirable item:"+ item.name);
            return -1;
        } 
        return inventoryItensMaxValues[item];
    }
    [Serializable]
    public struct InventoryItem
    {
        public ItemSO item;
        public int startingAmount;
        public int MaxAcquirable;
    }

    public bool IncreaseItemCapacity(ItemSO item,int amount =1)
    {
         if(!inventoryItens.ContainsKey(item))
        {
            Debug.LogError("Trying to acquire a non acquirable item:"+ item.name);
            return false;
        } 

        inventoryItensMaxValues[item] +=amount;
        onChangeItems?.Invoke(item);
        return true;

    }
}

