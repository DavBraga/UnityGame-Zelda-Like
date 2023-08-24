using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIVersionSetter : MonoBehaviour
{
    TextMeshProUGUI versionText;
    private void Awake() {
        versionText = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
