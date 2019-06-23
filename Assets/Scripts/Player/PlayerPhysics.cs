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

    void OnCollisionStay (Collision collision) {
        print("Collision enter");

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

            var velocityDifference = rb.velocity - playerMovement.velocity;
            var dot = Vector3.Dot(rb.velocity, collision.relativeVelocity);

            if (dot <= 0) {
                Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + rb.velocity.normalized, Color.blue, 60f);

                playerMovement.AddForce(collision.relativeVelocity * rb.mass);
            }

            rb.AddForceAtPosition(playerMovement.transform.forward * rigidBody.mass, collision.contacts[0].point);
        }
    }
}
