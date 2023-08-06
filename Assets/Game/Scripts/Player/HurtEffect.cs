using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HurtEffect : MonoBehaviour
{
    [SerializeField]Animator hurtVolume;
    Health myHealth;
    bool effectOn = false;
    private void Awake() {
        myHealth = GetComponent<Health>();
        myHealth.onTakeDamageNoParam+= PlayVolumeEffect;
        if(hurtVolume) effectOn = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayVolumeEffect()
    {
        if(effectOn)
        hurtVolume.SetTrigger("tHurt");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
