using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] GameObject exitButton;
    private void OnEnable() {
         if (Application.platform == RuntimePlatform.WindowsPlayer)
         exitButton.SetActive(true);
    }

    public void ExitCommand()
    {
        Application.Quit();
    }
}
