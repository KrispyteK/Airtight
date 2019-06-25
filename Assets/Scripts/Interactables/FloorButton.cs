using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour {
    public bool isPressed;
    public float pressDistance = 0.15f;
    public float speed = 10f;

    private Vector3 offset;

    public List<Collider> colliders = new List<Collider>();

    void Start () {
        offset = transform.localPosition;
    }

    void Update() {
        isPressed = colliders.Count > 0;

        var distance = isPressed ? pressDistance : 0;

        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + Vector3.down * distance, Time.deltaTime * speed);
    }

    //void OnTriggerStay (Collider collider) {
    //    if (collider.attachedRigidbody == null) return;

    //    collider.attachedRigidbody.AddForce(Vector3.down * 1000f);
    //}

    void OnTriggerEnter (Collider collider) {
        if (collider.attachedRigidbody == null) return;

        colliders.Add(collider);
    }

    void OnTriggerExit(Collider collider) {
        if (collider.attachedRigidbody == null) return;

        colliders.Remove(collider);
    }
}
