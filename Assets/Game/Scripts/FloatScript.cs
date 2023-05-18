using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScript : MonoBehaviour {
    public float underwaterDrag = 3f;
    public float underwaterAngularDrag = 1f;
    public float FloatingForce = 40;
    private Rigidbody thisRigidbody;
    private bool hasTouchedWater;

    private float basedrag;
    private float baseAngularDrag;

    // Start is called before the first frame update
    void Awake() {
        thisRigidbody = GetComponent<Rigidbody>();
    }
    private void Start() {
        basedrag = thisRigidbody.drag;
        baseAngularDrag = thisRigidbody.angularDrag;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Check if underwater
        float diffY = transform.position.y;
        bool isUnderwater = diffY < 0;
        if(isUnderwater) {
            hasTouchedWater = true;
        }

        // Ignore if never touched water
        if(!hasTouchedWater) {
            return;
        }

        // float logic
        if(isUnderwater) {
            Vector3 vector = Vector3.up * FloatingForce * -diffY;
            thisRigidbody.AddForce(vector, ForceMode.Acceleration);
        }
        thisRigidbody.drag = isUnderwater ? underwaterDrag : basedrag;
        thisRigidbody.angularDrag = isUnderwater ? underwaterAngularDrag : baseAngularDrag;
    }
}