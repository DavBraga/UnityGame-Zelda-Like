using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Zelda Like/Items/New Item", order = 0)]
public class ItemSO : ScriptableObject 
{
    [SerializeField] ItemTypeSO itemType;
    [SerializeField] string ItemName;
    [SerializeField] GameObject itemPrefab;

    public ItemTypeSO GetItemType()
    {
        return itemType;
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public GameObject GetItemPrefab()
    {
        return itemPrefab;
    }

}

