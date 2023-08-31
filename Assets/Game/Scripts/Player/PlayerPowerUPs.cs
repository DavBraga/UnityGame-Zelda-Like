using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class PlayerPowerUPs : MonoBehaviour
{
   [SerializeField] InventoryComunication inventoryComunication;
   [SerializeField] UIComunication uichannel; 
   [SerializeField] ItemSO item;

   private void Awake() {
   }

   private void OnEnable() {
        GetComponent<PlayerController>().onPowerUp+= ApplyPowerUp;
      
   }
   private void OnDisable() {
        GetComponent<PlayerController>().onPowerUp-= ApplyPowerUp;
   }
    public void ApplyPowerUp(PowerUpType type)
    {
        if(type == PowerUpType.bombTool)
        {
            GetComponent<BombTool>().LearnBombSkill();
            uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_bomb"));
            GameManager.Instance?.PauseGame();
        }
        else if(type == PowerUpType.damagePowerUp)
        {
            GetComponent<PlayerController>()?.onPowerIncrease.Invoke();
            uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUp_Damage"));
            GameManager.Instance?.PauseGame();
        }
        else if(type == PowerUpType.healthPowerUp)
        {
            GetComponent<Health>().IncreaseMaxHealth(5);
            uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_health"));
            GameManager.Instance?.PauseGame();
        }
        else if(type == PowerUpType.potionPowerUp)
        {
            inventoryComunication.IncreaseCarryCapacity(item);
            uichannel.ComunicatePowerUP(GetLocalizedString("key_powerUP_potionStack"));
            GameManager.Instance?.PauseGame();
            
        }
    }
    private string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("Hud", key);

    }
}