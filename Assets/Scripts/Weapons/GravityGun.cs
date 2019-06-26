using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour {

    public float holdForce = 15f;
    public float holdDamping = 1f;
    public float maxRange;
    public float bubbleShootOffset = 0.25f;
    public GameObject airBubble;
    public LayerMask castMask;
    public Rigidbody holding;

    private bool isHolding;
    private Quaternion rotation;
    private bool beforeGravity;
    private GameObject bubbleInstance;

    void Update() {

        if (Input.GetButtonDown("Fire2")) isHolding = true;
        if (Input.GetButtonUp("Fire2")) isHolding = false;

        if (Input.GetButtonDown("Fire1")) ShootAirBubble();

        if (holding == null) {
            if (isHolding) {
                var didHit = Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, maxRange, castMask);

                if (didHit) {
                    var rb = hit.collider.gameObject.GetComponent<Rigidbody>();

                    if (rb) {
                        holding = rb;

                        rotation = Quaternion.Inverse(transform.rotation) * rb.transform.rotation;

                        beforeGravity = holding.useGravity;

                        holding.freezeRotation = true;
                        holding.useGravity = false;

                        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), holding.GetComponent<Collider>(), true);
                        Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), holding.GetComponent<Collider>(), true);
                    }
                }
            }
        }
        else {
            if (Input.GetButtonUp("Fire2")) {
                StopHolding();
            }
            else if (Input.GetButton("Fire2")) {
                var targetPos = transform.position + transform.forward * holding.GetComponent<Collider>().bounds.size.magnitude - transform.up * 0.25f;
                var normal = (targetPos - holding.worldCenterOfMass);
                var distance = normal.magnitude;
                var powDistance = distance < 1f ? Mathf.Pow(distance, 0.65f) : Mathf.Pow(distance, 1.5f);

                normal = normal.normalized;

                holding.AddForce(
                        (normal * powDistance - holding.velocity * holdDamping) * holding.mass * holdForce,
                        ForceMode.Acceleration
                    );

                holding.MoveRotation(transform.rotation * rotation);
            }
        }
    }

    private void StopHolding() {
        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), holding.GetComponent<Collider>(), false);
        Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), holding.GetComponent<Collider>(), false);

        if (transform.forward.y < -0.7) holding.MovePosition(transform.position + Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized * holding.GetComponent<Collider>().bounds.size.magnitude);

        holding.useGravity = beforeGravity;
        holding.freezeRotation = false;
        holding = null;
        isHolding = false;
    }

    private void ShootAirBubble() {
        if (bubbleInstance) {
            bubbleInstance.GetComponent<AirBubble>().Kill();
        }
        else {
            var rayCast = Physics.SphereCast(transform.position, 0.25f, transform.forward, out RaycastHit hit, bubbleShootOffset, castMask);

            var position = transform.position;

            position += transform.forward * (rayCast ? hit.distance : bubbleShootOffset);

            bubbleInstance = Instantiate(airBubble, position, transform.rotation);
        }
    }
}
