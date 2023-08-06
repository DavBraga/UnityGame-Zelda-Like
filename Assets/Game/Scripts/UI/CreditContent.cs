
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CreditContent : MonoBehaviour
{
    public UnityEvent onFinish;
    [SerializeField] float animationDuration =4f;
    [SerializeField] float animationSpeed=1f;

    Animator myAnimator;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        myAnimator.speed= animationSpeed;
    }

    private void OnEnable() {
        StartCoroutine(Deactivate());
    }
    public void Finish()
    {
        onFinish?.Invoke();
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(animationDuration); 
        Finish();
    }
}
