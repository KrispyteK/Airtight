using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour {

    public float holdForce = 15f;
    public float shootForce = 25f;
    public float maxRange;
    public LayerMask castMask;
    public Rigidbody holding;

    private bool isHolding;

    void Start() {

    }

    void Update() {

        if (Input.GetButtonDown("Fire2")) isHolding = true;
        if (Input.GetButtonUp("Fire2")) isHolding = false;

        if (Input.GetButtonDown("Fire1")) Impulse();

        if (holding == null) {
            if (isHolding) {
                var didHit = Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, maxRange, castMask);

                if (didHit) {
                    var rb = hit.collider.gameObject.GetComponent<Rigidbody>();

                    if (rb) {
                        holding = rb;
                    }
                }
            }
        } else {
            if (Input.GetButtonDown("Fire1")) {
                holding.AddForce(
                        transform.forward * holding.mass * shootForce
                    );

                holding = null;
                isHolding = false;
            }
            else if (Input.GetButtonUp("Fire2")) {
                holding = null;
            }
            else if (Input.GetButton("Fire2")) {
                holding.AddForce(
                        ((transform.position + transform.forward - holding.worldCenterOfMass) - holding.velocity * 0.2f) * holding.mass * holdForce
                    );
            }
        }
    }

    private void Impulse () {
        var hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, maxRange, castMask);

        foreach (RaycastHit hit in hits) {
            var rb = hit.collider.gameObject.GetComponent<Rigidbody>();

            if (rb) {
                rb.AddForce(
                      transform.forward * rb.mass * shootForce
                 );
            }
        }
    }
}
