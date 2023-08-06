using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new InventoryComunication channel", menuName = "Zelda Like/Item/InventoryComunication", order = 0)]
public class InventoryComunication : ScriptableObject 
{
    PlayerInventory inventory;
    public bool AddItem(ItemSO item)
    {
       return inventory.AddItem(item);
    }
     public bool RemoveItem(ItemSO item)
    {
        return inventory.RemoveItem(item);
    }

    public bool AddItem(ItemSO item, int count)
    {
       return inventory.AddItem(item, count);
    }
     public bool RemoveItem(ItemSO item, int count)
    {
        return inventory.RemoveItem(item, count);
    }

    public void IncreaseCarryCapacity(ItemSO item)
    {
        inventory.IncreaseItemCapacity(item);
    }

    public int GetItemCount(ItemSO item)
    {
        return inventory.GetItemCount(item);
    }
       public int GetMAxItemCount(ItemSO item)
    {
        return inventory.GetMaxItemCount(item);
    }
    public void RegisterInventory(PlayerInventory playerInventory)
    {
        inventory = playerInventory;
    }
    public void UnregisterInventory(PlayerInventory playerInventory)
    {
        if(inventory!=playerInventory) return;
        
        inventory = null;
    }
}

