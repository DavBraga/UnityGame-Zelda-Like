using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionWidget : MonoBehaviour
{
    [SerializeField] string description = "InteractionEvent";
    [SerializeField] string interactionButton = "E";
    [SerializeField] TextMeshProUGUI actionGUI;
    [SerializeField] TextMeshProUGUI descriptionGUI;
    [SerializeField] Animator myAnimator;

    Coroutine hide;
    // Start is called before the first frame update
    void Start()
    {
        actionGUI.text = interactionButton;
        descriptionGUI.text = description;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hide()
    {
        
        myAnimator.SetBool("bOpen", false);
    }
    public void Show()
    {
        myAnimator.SetBool("bOpen", true);
    }

    //called on Animation Event
    public void DisableMyWidget()
    {
        //gameObject.SetActive(false);
    }

}
