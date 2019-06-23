using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAirVolume : MonoBehaviour {
    public float airRepel = 0.1f;
    public float force = 0.1f;
    public float maxForce = 10f;
    public LayerMask mask;

    private SphereCollider _collider;
    private Rigidbody _rigidBody;
    private float _realSize;

    void Start() {
        _collider = GetComponent<SphereCollider>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update() {
        Debug.DrawLine(transform.position, transform.position + _rigidBody.velocity, Color.green);

        _realSize = _collider.bounds.extents.magnitude;
    }

    void OnTriggerStay (Collider col) {
        var rb = col.GetComponent<Rigidbody>();

        if (rb != null && rb.gameObject != gameObject) {
            if (rb.isKinematic) return;

            var airVolume = rb.GetComponent<SphereAirVolume>();

            var referencePoint = col.ClosestPoint(transform.position);
            var dif = (referencePoint - transform.position);

            Debug.DrawLine(transform.position, referencePoint, Color.yellow);

            if (airVolume) {
                rb.AddForce(dif * airRepel);

                return;
            }

            var normalizedDistance = (dif.magnitude / _realSize);

            var cross = Vector3.Cross(dif, _rigidBody.velocity).normalized;

            var forceDir = Vector3.Cross(dif, cross) * normalizedDistance + (_rigidBody.velocity - rb.velocity) + Vector3.up;

            var forceVector = Vector3.ClampMagnitude((forceDir * force * (1 - normalizedDistance)) / _realSize, maxForce);

            Debug.DrawLine(rb.position, rb.position + forceVector.normalized, Color.blue);

            var airDrag = rb.GetComponent<AirDrag>();

            if (airDrag) {
                airDrag.AddForce(forceVector);
            }
        }
    }
}
