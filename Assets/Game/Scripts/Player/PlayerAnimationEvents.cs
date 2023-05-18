using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    IEnumerator delayedDisable;
    [SerializeField] GameObject atackCollider;
    [SerializeField] float delay= 0.06f;
    private void Start() {
        delayedDisable = DelayedDisable(delay);
    }
    public void EnableAtackCollider()
    {
        StopCoroutine(delayedDisable);
        atackCollider.SetActive(true);
        StartCoroutine(delayedDisable);
    }
    public void DisableAtackCollider()
    {
        atackCollider.SetActive(false);

    }
    IEnumerator DelayedDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableAtackCollider();
    }
}
