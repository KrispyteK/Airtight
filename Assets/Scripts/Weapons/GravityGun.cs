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
    private Quaternion rotation;

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

                        rotation = Quaternion.Inverse(transform.rotation) * rb.transform.rotation;

                        holding.freezeRotation = true;
                        holding.useGravity = false;

                        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), holding.GetComponent<Collider>(), true);
                        Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), holding.GetComponent<Collider>(), true);
                    }
                }
            }
        } else {
            if (Input.GetButtonDown("Fire1")) {
                holding.AddForce(
                        transform.forward * shootForce
                    );

                StopHolding();
            }
            else if (Input.GetButtonUp("Fire2")) {
                StopHolding();
            }
            else if (Input.GetButton("Fire2")) {
                holding.AddForce(
                        ((transform.position + transform.forward * holding.GetComponent<Collider>().bounds.size.magnitude - holding.worldCenterOfMass) - holding.velocity * 0.2f) * holding.mass * holdForce
                    );

                holding.MoveRotation(transform.rotation * rotation);
            }
        }
    }

    private void StopHolding () {
        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), holding.GetComponent<Collider>(), false);
        Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), holding.GetComponent<Collider>(), false);

        if (transform.forward.y < -0.7) holding.MovePosition(transform.position + Vector3.Scale(transform.forward,new Vector3(1,0,1)).normalized * holding.GetComponent<Collider>().bounds.size.magnitude);

        holding.useGravity = true;
        holding.freezeRotation = false;
        holding = null;
        isHolding = false;
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
