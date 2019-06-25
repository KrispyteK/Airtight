using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDrag : MonoBehaviour {

    public float drag = 1f;
    
    private BoxCollider col;
    private Rigidbody rb;

    public Vector3[] _normals = new[]{
            new Vector3(1,0,0),
            new Vector3(-1,0,0),
            new Vector3(0,1,0),
            new Vector3(0,-1,0),
            new Vector3(0,0,1),
            new Vector3(0,0,-1)
        };

    public float[] _surfaces;
    public Vector3[] _offsets;

    void Start() {
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 vel;
    private Vector3 _force;
    private int _appliedForces;

    public void AddForce (Vector3 force) {
        _force += force;

        _appliedForces++;
    }

    void Update() {
        Vector3 force = Vector3.zero;
        Vector3 torque = Vector3.zero;

        if (_force.magnitude > 0 || _appliedForces > 0) {
            vel += -_force / _appliedForces;
            _force *= 0;
            _appliedForces = 0;
        }

        for (int i = 0; i < 6; i++) {
            Vector3 normal = transform.TransformDirection(_normals[i]).normalized;
            float dot = Vector3.Dot(normal, vel);

            if (dot > 0.1f) {
                Vector3 f = -normal * dot * _surfaces[i];
                Vector3 point = transform.TransformPoint(_normals[i] / 2);
                Vector3 axis = Vector3.Cross(normal, vel).normalized;
                float normalizedDot = dot / vel.magnitude;

                force += f;
                torque += axis * Mathf.Pow((1 - normalizedDot),2) * dot * drag * Mathf.Pow(_surfaces[i],2);

                //Debug.DrawLine(point, point + axis, Color.red);
            }
        }

        rb.AddForce(force);
        rb.AddTorque(torque);

        vel = rb.velocity;
    }

    void OnValidate () {
        col = GetComponent<BoxCollider>();
        GetSurfaces(col);
    }

    private void GetSurfaces (BoxCollider collider) {
        var size = Vector3.Scale(collider.size,transform.localScale);

        _surfaces = new[]{
            size.x * size.y,
            size.x * size.y,
            size.z * size.x,
            size.z * size.x,
            size.y * size.z,
            size.y * size.z
        };

        _offsets = new[]{
            _normals[0] * size.z,
            _normals[1] * size.z,
            _normals[2] * size.y,
            _normals[3] * size.y,
            _normals[4] * size.x,
            _normals[5] * size.x
        };
    }
}
