using UnityEngine;

public class Attack : MonoBehaviour
{
  [SerializeField] bool leftHand = false;
  [SerializeField] PlayerController controller;
    private void OnTriggerEnter(Collider other) {
        controller.TryTriggerHandAttack(other, leftHand);
    }
}
