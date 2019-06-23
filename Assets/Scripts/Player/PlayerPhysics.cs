using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {

    private Movement playerMovement;

    void Awake () {
        playerMovement = GetComponent<Movement>();
    }

    void OnCollisionStay (Collision collision) {
        print("Collision enter");

        Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + collision.contacts[0].normal, Color.red, 60f);

        var rb = collision.collider.GetComponent<Rigidbody>();

        if (rb) {
            print(collision.relativeVelocity);

            playerMovement.AddForce(-collision.relativeVelocity * rb.mass);
            rb.AddForceAtPosition((playerMovement.velocity + collision.relativeVelocity) * rb.mass, collision.contacts[0].point);
        }
    }
}
