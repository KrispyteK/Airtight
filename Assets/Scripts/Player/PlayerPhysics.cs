using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    public float pushForce = 5f;

    private Movement playerMovement;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;

    void Awake () {
        playerMovement = GetComponent<Movement>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        var rb = hit.collider.attachedRigidbody;

        // no rigidbody
        if (rb == null || rb.isKinematic) return;

        //var m1 = playerMovement.rigidbody.mass;
        //var m2 = rb.mass;
        //var u1 = playerMovement.velocity;
        //var u2 = rb.velocity;

        //var v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;
        //var v2 = ((m2 - m1) / (m1 + m2)) * u2 + ((2 * m1) / (m1 + m2)) * u1;

        //print(v1);

        //playerMovement.AddForce(v1 * m2);
        //rb.AddForceAtPosition(v2, hit.point);

        rb.AddForceAtPosition(hit.moveDirection * playerMovement.rigidbody.mass * pushForce, hit.point);

        rb.velocity += playerMovement.velocity / rb.mass;
    }

    void OnCollisionEnter (Collision collision) {
        var rb = collision.collider.attachedRigidbody;

        // no rigidbody
        if (rb == null || rb.isKinematic) return;

        var m1 = playerMovement.rigidbody.mass;
        var m2 = rb.mass;
        var u1 = playerMovement.velocity;
        var u2 = collision.relativeVelocity;

        var v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;

        playerMovement.AddForce(v1 * m2);
    }

    void OnCollisionStay (Collision collision) {
        var rb = collision.collider.GetComponent<Rigidbody>();

        if (rb) {
            Transform other = rb.transform;

            bool overlapped = Physics.ComputePenetration(
                capsuleCollider, transform.position, transform.rotation,
                collision.collider, other.position, other.rotation,
                out Vector3 direction, out float distance
            );

            if (overlapped) {
                Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + direction * distance, Color.red, 60f);

                playerMovement.Move(direction * distance);
            }
        }
    }
}
