using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirProjectile : MonoBehaviour {

    public float lift = 0.1f;
    public float startSize = 0.25f;
    public float scale = 10f;
    public float velocityScalar = 5f;
    public LayerMask mask;

    private float _startForce;
    private float _size;
    private Vector3 _velocity;
    private Rigidbody _rigidBody;
    private SphereAirVolume _airVolume;

    void Start() {
        _rigidBody = GetComponent<Rigidbody>();
        _airVolume = GetComponent<SphereAirVolume>();

        _startForce = _airVolume.force;
        _velocity = transform.forward * velocityScalar;

        _rigidBody.velocity = _velocity;

        _size = startSize;
    }

    void Update() {
        _size += Time.deltaTime * 1/_rigidBody.velocity.magnitude * scale;

        transform.localScale = Vector3.one * _size;

        if (_rigidBody.velocity.magnitude < 0.01f || _size > 5f) {
            Destroy(gameObject);
        }
    }
}
