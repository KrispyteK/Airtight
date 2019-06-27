using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public float radius = 0.1f;
    public float distance = 1.5f;
    public LayerMask layerMask;
    public Transform cameraTransform;

    void Update() {
        if (Input.GetButtonDown("Interact")) {
            var rayCast = RayCast(out RaycastHit hit);

            if (rayCast) {
                var interactable = hit.collider.GetComponent<Interactable>();

                if (interactable) {
                    interactable.onStartInteract.Invoke();
                }
            }
        }
    }

    public bool RayCast (out RaycastHit hit) {
        return Physics.SphereCast(cameraTransform.position, radius, cameraTransform.forward, out hit, distance, layerMask);
    }
}
