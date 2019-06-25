using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBubble : MonoBehaviour {

    public float maxSize = 2f;
    public float size = 0f;
    public float growTime = 0.5f;
    public float velocity = 4f;
    public float force = 10f;

    public LayerMask layerMask;
    public LayerMask overlapMask;

    private SphereCollider sphereCollider;
    private List<Collider> colliders = new List<Collider>();

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update() {
        var rayCast = Physics.SphereCast(transform.position, size, transform.forward, out RaycastHit hit, velocity * Time.deltaTime, layerMask);
        var overlap = Physics.CheckSphere(transform.position, Mathf.Clamp(size + Time.deltaTime / growTime, 0,maxSize), overlapMask);

        size = Mathf.Clamp(size + Time.deltaTime / growTime * (overlap ? -1 : 1), 0, maxSize);

        sphereCollider.radius = size;

        transform.position = transform.position + transform.forward * velocity * Time.deltaTime;

        if (rayCast && Vector3.Dot(-hit.normal, transform.forward) > 0.8f) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.attachedRigidbody == null || collider.attachedRigidbody.isKinematic) return;

        collider.attachedRigidbody.velocity *= 0;

        colliders.Add(collider);
    }

    void OnTriggerExit(Collider collider) {
        colliders.Remove(collider);
    }

    void OnTriggerStay (Collider collider) {
        if (collider.attachedRigidbody == null || collider.attachedRigidbody.isKinematic) return;

        collider.attachedRigidbody.AddForce((
            (transform.position - collider.transform.position) 
            + transform.forward * velocity 
            - collider.attachedRigidbody.velocity
            ) * force
            , ForceMode.Acceleration);
    }
    void OnDestroy() {
        foreach (var collider in colliders) {
            if (collider.attachedRigidbody == null || collider.attachedRigidbody.isKinematic) return;

            collider.attachedRigidbody.velocity *= 0;
        }
    }
}
