using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public Transform cameraTransform;

    public bool isAlive = true;

    public void TakeDamage (int amount) {
        health = Mathf.Max(health - amount,0);

        if (health == 0) {
            Kill();
        }
    }

    public void Kill () {
        if (!isAlive) return;

        isAlive = false;

        cameraTransform.parent = null;

        cameraTransform.GetComponent<PlayerCamera>().enabled = false;
        cameraTransform.GetComponent<GravityGun>().enabled = false;

        var col = cameraTransform.gameObject.AddComponent<SphereCollider>();

        col.radius = 0.25f;

        var rb = cameraTransform.gameObject.AddComponent<Rigidbody>();

        rb.AddForce(Vector3.down);
        rb.AddTorque(Random.insideUnitSphere);

        gameObject.SetActive(false);
    }
}
