using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInteraction))]
public class PlayerPickup : MonoBehaviour {

    public Rigidbody holding;
    public ConfigurableJoint joint;
    public Transform cameraTransform;
    public float depression = -35f;
    public float distance = 1f;
    public bool collision = false;

    private PlayerInteraction playerInteraction;
    private Collider col;

    void Start() {
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        col = GetComponent<Collider>();
    }

    void Update() {


        if (holding) {
            Vector3 cameraForward = cameraTransform.forward;
            float angle = Vector3.Angle(cameraForward, Vector3.up);

            if (angle > 90f + depression) cameraForward = (col.transform.forward + new Vector3(0, -Mathf.Sin(depression * Mathf.Deg2Rad), 0)).normalized;

            Vector3 worldAnchor = joint.transform.InverseTransformPoint(col.ClosestPoint(cameraForward + cameraTransform.position) + cameraForward * distance);
            Vector3 holdingPos = joint.transform.InverseTransformPoint(holding.position);
            Vector3 collisionPos = joint.transform.InverseTransformPoint(holding.GetComponent<Collider>().ClosestPoint(cameraTransform.position));
            Vector3 offset = holdingPos - collisionPos;

            Vector3 anchor = worldAnchor + new Vector3(0, 0, offset.z);

            joint.targetPosition = anchor;

            //if (playerInteraction.RayCast(out RaycastHit hit) && hit.collider.attachedRigidbody != holding) Drop();
        }

        if (Input.GetButtonDown("Interact") && holding == null) {
            var rayCast = playerInteraction.RayCast(out RaycastHit hit);

            if (rayCast) {
                if (hit.collider.attachedRigidbody) {
                    joint.connectedBody = holding = hit.collider.attachedRigidbody;

                    holding.useGravity = false;

                    SetCollisions(collision);
                }
            }
        }
        else if (Input.GetButtonDown("Interact") && holding) {
            Drop();
        }
    }

    public void Drop () {
        if (!holding) return;

        SetCollisions(true);

        holding.useGravity = true;

        joint.connectedBody = holding = null;
    }

    private void SetCollisions(bool on) {
        Physics.IgnoreCollision(transform.root.GetComponent<CharacterController>(), holding.GetComponent<Collider>(), !on);
        Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), holding.GetComponent<Collider>(), !on);
    }
}
