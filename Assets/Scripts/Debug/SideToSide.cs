using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideToSide : MonoBehaviour {

    public Vector3 direction;
    public float speed = 2f;
    public float distance = 1f;

    private Vector3 _startPos;
    private float _t;
    private Rigidbody rb;

    void Start() {
        _startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        _t += Time.deltaTime * speed;

        rb.MovePosition(_startPos + direction * Mathf.Sin(_t) * distance);
    }
}
