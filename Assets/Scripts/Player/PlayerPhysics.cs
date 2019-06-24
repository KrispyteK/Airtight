using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    private Movement playerMovement;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;

    void Awake () {
        playerMovement = GetComponent<Movement>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void OnCollisionEnter(Collision collision) {
        var rb = collision.collider.GetComponent<Rigidbody>();

        print("Collision enter: " + collision.impulse.magnitude);

        if (rb) {
            var m1 = playerMovement.rigidbody.mass;
            var m2 = rb.mass;
            var u1 = playerMovement.velocity;
            var u2 = collision.relativeVelocity;

            var v1 = ((m1 - m2) / (m1 + m2)) * u1 + ((2 * m2) / (m1 + m2)) * u2;
            var v2 = ((m2 - m1) / (m1 + m2)) * u2 + ((2 * m1) / (m1 + m2)) * u1;

            playerMovement.AddForce(v1 * m2);
            rb.AddForceAtPosition((v2 - rb.velocity) / 2, collision.contacts[0].point, ForceMode.VelocityChange);
        } 
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
