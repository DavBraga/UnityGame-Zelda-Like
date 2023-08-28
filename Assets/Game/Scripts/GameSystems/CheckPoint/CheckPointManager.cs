using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] InventoryComunication inventoryChannel;
    [SerializeField] ItemSO potions;
    [SerializeField] FadeEffect fader;
    [SerializeField] string lastCheckpoint= "";
    int potionCount= 0;
    Transform returnPostion;
    PlayerAvatar player;
    Coroutine restorationRoutine;

    UnityAction eventONRestore;

    private void Awake() {
        player = GetComponent<PlayerAvatar>();
    }
    public void SaveCheckPoint(Transform respawnPosition,int potions, string checkpointName)
    {
        Debug.Log("checkpoint saved");
        lastCheckpoint = checkpointName;
        returnPostion = respawnPosition;
        potionCount = potions;
    }
    public void SaveCheckPoint(Transform respawnPosition,int potions, UnityAction specialEvent, string checkpointName)
    {
        Debug.Log("checkpoint saved");
        eventONRestore = null;
        eventONRestore +=specialEvent;
        lastCheckpoint = checkpointName;
        returnPostion = respawnPosition;
        potionCount = potions;
    }

    
    public void RestorePlayer()
    {
        transform.SetPositionAndRotation(returnPostion.position,returnPostion.rotation); //
        if(restorationRoutine!=null) StopCoroutine(restorationRoutine);
        StartCoroutine(WaitAndRestore());
    }
    IEnumerator WaitAndRestore()
    {
        yield return new WaitForSeconds(1.5f);
        eventONRestore?.Invoke();
        if(lastCheckpoint == "boss")
        { 
            yield return new WaitForSeconds(5.0f);
            inventoryChannel.RemoveItem(potions,99);
            inventoryChannel.AddItem(potions,99);
        }
        fader.FadeIn();
        player.onRessurect?.Invoke();
        GameManager.Instance?.ChangeGameState(GameState.playing);
    }
}
