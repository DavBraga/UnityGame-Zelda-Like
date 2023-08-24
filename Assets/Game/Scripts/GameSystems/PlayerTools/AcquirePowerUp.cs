using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Events;


public class AcquirePowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    [SerializeField]InventoryComunication inventoryComunication;
    [SerializeField] UIComunication uichannel;
    [SerializeField]ItemSO item;
    public UnityAction<PowerUpType> onPowerUp;

    private void Awake() {
        if(powerUpType==PowerUpType.bombTool)
        GetComponent<InteractiveObject>().onInteraction += AddBombTool;

        

        else if(powerUpType==PowerUpType.damagePowerUp)
        GetComponent<InteractiveObject>().onInteraction += AddPower;
            
        
        

        else if(powerUpType==PowerUpType.healthPowerUp)
        GetComponent<InteractiveObject>().onInteraction += AddHealth;
        
        
        else if(powerUpType==PowerUpType.potionPowerUp)
        GetComponent<InteractiveObject>().onInteraction += AddMaxPotion;
        
    }
    private string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("Hud", key);

    }
    public void AddBombTool(GameObject player)
    {
        player.GetComponent<BombTool>().LearnBombSkill();
        //uichannel.ComunicatePowerUP("Bomb tool Acquired!");
        uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_bomb"));
        GameManager.Instance?.PauseGame();
    }
    public void AddPower(GameObject player)
    {
       player.GetComponent<PlayerController>().IncreaseAttackPower();
       // uichannel.ComunicatePowerUP("Attack power increased by 1!");
        uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUp_Damage"));
        GameManager.Instance?.PauseGame();
 
    }
     public void AddHealth(GameObject player)
    {
       player.GetComponent<Health>().IncreaseMaxHealth(5);
       //uichannel.ComunicatePowerUP("Max health increased by 5");
       uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_health"));
       GameManager.Instance?.PauseGame();
    }
      public void AddMaxPotion(GameObject player)
    {
       inventoryComunication.IncreaseCarryCapacity(item);
        //uichannel.ComunicatePowerUP("Max potion stack increased by 1");
        uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_potionStack"));
        GameManager.Instance?.PauseGame();
    }
}


public enum PowerUpType
{
    bombTool,
    damagePowerUp,
    potionPowerUp,
    healthPowerUp,
}
