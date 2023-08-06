using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new UI channel", menuName = "Zelda Like/Item/UIComunication", order = 0)]

public class UIComunication : ScriptableObject
{
    HudHandler hud;

    public void ComunicatePowerUP(string message)
    {
        hud.SetUpPowerUpMessage(message);
    }

    public void  RegisterUI(HudHandler hudHandler)
    {
        hud = hudHandler;
    }
}
