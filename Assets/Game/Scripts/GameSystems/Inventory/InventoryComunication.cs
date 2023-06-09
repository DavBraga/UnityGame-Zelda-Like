using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new InventoryComunication channel", menuName = "Zelda Like/Item/InventoryComunication", order = 0)]
public class InventoryComunication : ScriptableObject 
{
    PlayerInventory inventory;
    public void AddItem(ItemSO item)
    {
        inventory.AddItem(item);
    }
     public bool RemoveItem(ItemSO item)
    {
        return inventory.RemoveItem(item);
    }

    public int GetKeyCount()
    {
        return inventory.GetKeyCount();
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

