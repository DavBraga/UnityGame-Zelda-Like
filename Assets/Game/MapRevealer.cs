using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRevealer : MonoBehaviour
{
    SpriteRenderer mySprite;
    [SerializeField] Color myColor;
    [SerializeField] bool startHidden = true;
    bool amIhidden = false;

    private void Awake() {
        mySprite = GetComponent<SpriteRenderer>();

    }
    private void OnEnable() {
        GetComponent<OnTouchInteraction>().onTouch+= Reveal;
    }
    private void OnDisable() {
        GetComponent<OnTouchInteraction>().onTouch-= Reveal;
        
    }
    private void Start() {
        if(startHidden) Hide();
    }
    public void Reveal()
    {
        if(!amIhidden) return;
        mySprite.color = myColor;
        amIhidden = false;
    }
    public void Hide()
    {
        myColor = mySprite.color;
        mySprite.color = Color.clear;
        amIhidden = true;
    }
}
