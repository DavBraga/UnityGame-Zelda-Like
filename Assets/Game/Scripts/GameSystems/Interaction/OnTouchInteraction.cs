using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTouchInteraction : MonoBehaviour
{
    public UnityAction onTouch;

    private void OnTriggerEnter(Collider other) {
        onTouch.Invoke();
    }
}
