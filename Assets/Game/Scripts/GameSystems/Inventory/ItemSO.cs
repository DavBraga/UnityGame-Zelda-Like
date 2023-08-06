using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Zelda Like/Items/New Item", order = 0)]
public class ItemSO : ScriptableObject 
{
    [SerializeField] string ItemName;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] GameObject pickAble;
    [SerializeField] AudioClip acquireSound;

    bool gotSound;

    private void OnEnable() {
        if(acquireSound)
        gotSound = true;
        else gotSound = false;
    }
    public string GetItemName()
    {
        return ItemName;
    }

    public bool GotSound()
    {
        return gotSound;
    }
    public AudioClip GetSound()
    {
        return acquireSound;
    }
    public GameObject GetPickable()
    {
        return pickAble;
    }
    public GameObject GetItemPrefab()
    {
        return itemPrefab;
    }

    virtual public bool UseItem(GameObject user)
    {
        return false;
    }

}

