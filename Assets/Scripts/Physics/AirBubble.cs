using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class AirBubble : MonoBehaviour {

    public AnimationCurve sizeCurve;
    public float maxSize = 2f;
    public float size = 0f;
    public float growTime = 0.5f;
    public float sizeChangeSmoothing = 10f;
    public float velocity = 4f;
    public float force = 10f;

    public LayerMask layerMask;
    public LayerMask overlapMask;

    public VisualEffect visualEffect;

    private SphereCollider sphereCollider;
    private List<Collider> colliders = new List<Collider>();
    private float t;
    private float smoothSize;

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update() {
        t = Mathf.Min(t + Time.deltaTime, sizeCurve.keys[sizeCurve.length - 1].time);

        var _maxSize = maxSize * sizeCurve.Evaluate(t);

        var rayCast = Physics.SphereCast(transform.position, 0.25f, transform.forward, out RaycastHit hit, velocity * Time.deltaTime, layerMask);
        var overlap = Physics.CheckSphere(transform.position, Mathf.Clamp(size + Time.deltaTime / growTime, 0, _maxSize), overlapMask);

        size = Mathf.Clamp(size + Time.deltaTime / growTime * (overlap ? -1 : 1), 0, _maxSize);

        smoothSize = Mathf.Lerp(size,smoothSize, sizeChangeSmoothing * Time.deltaTime);

        visualEffect.SetFloat("Radius", smoothSize);

        transform.localScale = Vector3.one * smoothSize * 2;

        transform.position = transform.position + transform.forward * velocity * Time.deltaTime;

        if (rayCast) {
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
