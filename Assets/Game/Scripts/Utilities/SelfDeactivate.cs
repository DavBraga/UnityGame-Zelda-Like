using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    [SerializeField] float aftertime = 1f;
    float enabletime = 0f;

    private void OnEnable() {
        enabletime = Time.time;
    }
    private void Update() {
        if(Time.time>enabletime+aftertime)
            gameObject.SetActive(false);
    }
}
