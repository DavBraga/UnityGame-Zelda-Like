using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics.CodeAnalysis;

public class AcquirePowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    [SerializeField]InventoryComunication inventoryComunication;
    [SerializeField] UIComunication uichannel;
    [SerializeField]ItemSO item;
    public UnityAction<GameObject> onPowerUp;

    private void Awake() {

       GetComponent<InteractiveObject>().onInteraction +=
       (GameObject player)=> 
       player.GetComponent<PlayerAvatar>().OnPowerUp.Invoke(powerUpType);
    }
}
public enum PowerUpType
{
    bombTool,
    damagePowerUp,
    potionPowerUp,
    healthPowerUp,
}
