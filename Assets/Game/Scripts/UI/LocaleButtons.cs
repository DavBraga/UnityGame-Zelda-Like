using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleButtons : MonoBehaviour
{
    [SerializeField] Locale myLocale;
    CanvasGroup myCanvas;
    private void Awake() {
        myCanvas = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        UpdateLocale();
    }

    public void UpdateLocale()
    {
        myCanvas.alpha = LocalizationSettings.SelectedLocale == myLocale ? 1 : 0.1f;
    }

    public void ChangeLocale()
    {
        LocalizationSettings.SelectedLocale = myLocale;
    }
}
