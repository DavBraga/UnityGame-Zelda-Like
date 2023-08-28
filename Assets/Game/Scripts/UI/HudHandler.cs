using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudHandler : MonoBehaviour
{
    [SerializeField]UIComunication mychannel;
    [Header("Player")]
    [SerializeField] PlayerAvatar player;
    [SerializeField] BarHandler playerBar;
    [SerializeField] Health playerHealth;
    [SerializeField] PowerUpMessages powerUpMessages;

    [Header("Boss")]
    [SerializeField] GameObject bossBarGameObj;
    [SerializeField] BarHandler bossBar;
    [SerializeField] Health bossHealth;

    [Header("Potions")]
    [SerializeField]ItemSO potions;
    [SerializeField] TextMeshProUGUI potionsText;
    [SerializeField] Animator potionButton;

    [Header("Keys")]
    [SerializeField]ItemSO keys;
    [SerializeField] TextMeshProUGUI KeysText;
    
    [Header("BombButton")]

    [SerializeField] Animator bombButtonAnimator;
    [SerializeField] Animator bombNonVirtual;
    [SerializeField] PlayerInventory inventory;

    [Header("Animator")]
    [SerializeField] Animator myAnimator;
    [SerializeField] string ShowHudTrigger = "THudFadeIn";
    [SerializeField] string HideHudTrigger = "THudFadeOut";

    [SerializeField] VirtualInputSetter virtualInputSetter;
    bool virtualHud = false;
    float delay = 1f;
    float startTime = 0;
    bool isloaded = false;

    private void Awake()
    {
        virtualInputSetter.onVirtualInputToggles+= SetVirtualHud;
        playerHealth.onChangeHealth += () => { 
            playerBar.UpdateMaxValue (playerHealth.GetMaxHealth()); 
            playerBar.UpdateBarValue(playerHealth.GetCurrentHealth()); 
            };
       // 
        SetUpBombButton();
        SetUpPotionButton();
        myAnimator = GetComponent<Animator>();
        inventory.onChangeItems += UpdateItensOnHud;
        mychannel.RegisterUI(this);
    }

    public void SetVirtualHud(bool isOn)
    {
        virtualHud = isOn;

    }

    private void SetUpBombButton()
    {
        BombTool bombTool = player.GetComponent<BombTool>();
        bombTool.onPlaceBomb += BOmbBUttonCooldown;
        bombTool.onBombCooldownEnds += BOmbBUttonReady;
        bombTool.onLearnBombTool += EnableBombButton;
    }
    public void EnableBombButton()
    {
        if(virtualHud)
        bombButtonAnimator.gameObject.SetActive(true);
        else
        bombNonVirtual.gameObject.SetActive(true);
    }

       private void SetUpPotionButton()
    {
        UsePotion potions = player.GetComponent<UsePotion>();
        potions.onPotionUse += ()=>{potionButton.SetTrigger("tCooldown");};
        potions.onPotionCooldownEnds += ()=>{potionButton.SetTrigger("tReady");};
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime =Time.time;
        virtualHud = PlayerPrefs.GetInt("usingVirtualInput")>0;
    }

    public void UpdateItensOnHud(ItemSO itemtype)
    {
        if(itemtype==potions)
           potionsText.text = inventory.GetItemCount(potions)+"/"+inventory.GetMaxItemCount(potions);
        else KeysText.text = "x"+inventory.GetItemCount(keys);
    }

    public void BOmbBUttonReady()
    {
        if(virtualHud)
        bombButtonAnimator.SetTrigger("tReady");
        else
        bombNonVirtual.SetTrigger("tReady");
    }

    public void BOmbBUttonCooldown()
    {
        if(virtualHud)
        bombButtonAnimator.SetTrigger("tCooldown");
        else
        bombNonVirtual.SetTrigger("tCooldown");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time<startTime+delay) return;
        if(!isloaded)
        {
            UpdateHUD();
            isloaded = true;
        }
    }

    private void UpdateHUD()
    {
        playerBar.InitializeValues(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
        bossBar.InitializeValues(bossHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    public void WakeBossHud()
    {
        bossBarGameObj.SetActive(true);
        bossBar.gameObject.SetActive(true);
        bossBar.InitializeValues(bossHealth.GetCurrentHealth(), bossHealth.GetMaxHealth());
        bossHealth.onChangeHealth += () => { bossBar.UpdateBarValue(bossHealth.GetCurrentHealth()); };
    }

    public void HideHud()
    {
        Debug.Log("Fadeout call");
        myAnimator.SetTrigger("tHudFadeOut");
    }
    public void ShowHud()
    {
        Debug.Log("FadeIN call");
        myAnimator.SetTrigger("tHudFadeIn");
    }

    public void SetUpPowerUpMessage(string message)
    {
        powerUpMessages.gameObject.SetActive(true);
        powerUpMessages.SetUpMessage(message);
    }
    private void OnEnable() {
        
    }
    public void ReloadHud()
    {

    }
}

