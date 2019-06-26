using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : Interactable {
    public bool isPressed;
    public float pressDistance = 0.15f;
    public float speed = 10f;

    private Vector3 offset;
    private bool wasPressed;

    public List<Collider> colliders = new List<Collider>();

    void Start () {
        offset = transform.localPosition;
    }

    void Awake () {
        StartCoroutine(CheckColliders());
    }

    void Update() {
        wasPressed = isPressed;

        isPressed = colliders.Count > 0;

        if (isPressed != wasPressed) {
            if (isPressed) {
                onStartInteract.Invoke();
            } else {
                onEndInteract.Invoke();
            }
        }

        var distance = isPressed ? pressDistance : 0;

        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + Vector3.down * distance, Time.deltaTime * speed);
    }

    IEnumerator CheckColliders () {
        while (true) {
            for (int i = colliders.Count - 1; i > -1; i--) { 
                if (colliders[i] == null) {
                    colliders.RemoveAt(i);
                }
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.attachedRigidbody == null) return;

        colliders.Add(collider);
    }

    void OnTriggerExit(Collider collider) {
        if (collider.attachedRigidbody == null) return;

        colliders.Remove(collider);
    }
}
