using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class AirBubble : MonoBehaviour {

    public AnimationCurve sizeCurve;
    public float maxPenetration = 0.2f;
    public float minSize = 0.25f;
    public float maxSize = 2f;
    public float size = 0f;
    public float growTime = 0.5f;
    public float sizeChangeSmoothing = 10f;
    public float velocity = 4f;
    public float force = 10f;
    public float explosionTime = 0.25f;

    public LayerMask layerMask;
    public LayerMask overlapMask;

    public VisualEffect visualEffect;
    public GameObject explosion;

    private SphereCollider sphereCollider;
    public List<Collider> colliders = new List<Collider>();
    private float t;
    private float smoothSize;

    void Start() {
        sphereCollider = GetComponent<SphereCollider>();

        size = minSize;
    }

    void Update() {
        CheckColliders();

        t = Mathf.Min(t + Time.deltaTime, sizeCurve.keys[sizeCurve.length - 1].time);

        var _minSize = Mathf.Max(GetSmallestBounds().extents.magnitude + minSize, minSize);
        var _maxSize = maxSize * sizeCurve.Evaluate(t);

        var rayCast = Physics.SphereCast(transform.position, _minSize - maxPenetration, transform.forward, out RaycastHit hit, velocity * Time.deltaTime, layerMask);
        var overlap = Physics.CheckSphere(transform.position, Mathf.Clamp(size + Time.deltaTime / growTime, 0, _maxSize), overlapMask);

        size = Mathf.Clamp(size + Time.deltaTime / growTime * (overlap ? -1 : 1), 0, _maxSize);

        smoothSize = Mathf.Clamp(smoothSize + (size - smoothSize) * sizeChangeSmoothing * Time.deltaTime, _minSize, _maxSize);

        visualEffect.SetFloat("Radius", smoothSize);

        transform.localScale = Vector3.one * smoothSize * 2;

        transform.position = transform.position + transform.forward * velocity * Time.deltaTime;

        if (rayCast) {
            Destroy(gameObject);
        }
    }

    private void CheckColliders() {
        for (int i = colliders.Count - 1; i > -1; i--) {
            if (colliders[i] == null) {
                colliders.RemoveAt(i);
            }
        }
    }

    private Bounds GetSmallestBounds () {
        if (colliders.Count == 0) return default; 

        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        for (int i = 0; i < colliders.Count; i++) {
            var dif = (colliders[i].transform.position - transform.position);
            var furthestPoint = colliders[i].ClosestPoint(transform.position + dif.normalized * 1000f);

            Debug.DrawLine(transform.position, furthestPoint, Color.red);

            bounds.Encapsulate(furthestPoint);
        }

        print(bounds);

        return bounds;
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
    
    public void Kill () {
        StartCoroutine(DestroyRoutine());
    }

    void OnDestroy() {
        foreach (var collider in colliders) {
            if (collider.attachedRigidbody == null || collider.attachedRigidbody.isKinematic) return;

            collider.attachedRigidbody.velocity *= 0;
        }

        Instantiate(explosion, transform.position,transform.rotation);
    }

    IEnumerator DestroyRoutine () {
        float t = 1f;

        while (t > 0) {
            size *= t;
            velocity *= t;

            t = Mathf.Clamp01(t - Time.deltaTime / explosionTime);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
