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

    void OnCollisionEnter (Collision collision) {
        var rb = collision.collider.GetComponent<Rigidbody>();

        if (rb) {
            var velObject = collision.relativeVelocity;
            var fKineticPlayer = 0.25f * rb.mass * (velObject.normalized * Mathf.Pow(velObject.magnitude, 2));
            var velPlayer = playerMovement.velocity;
            var fKineticObject = 0.25f * playerMovement.rigidbody.mass * (velPlayer.normalized * Mathf.Pow(velPlayer.magnitude, 2));

            playerMovement.AddForce(fKineticPlayer);

            print("Collision vel: " + velObject.magnitude);
            print("Player vel: " + playerMovement.velocity.magnitude);

            rb.AddForceAtPosition(fKineticObject, collision.contacts[0].point);
        }
    }

    void OnCollisionStay (Collision collision) {
        print("Collision stay");

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
