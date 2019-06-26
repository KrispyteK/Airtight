using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    public float distance = 1.5f;
    public LayerMask layerMask;

    void Update() {
        if (Input.GetButtonDown("Interact")) {
            var rayCast = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance, layerMask);

            if (rayCast) {
                var interactable = hit.collider.GetComponent<Interactable>();

                if (interactable) {
                    interactable.onStartInteract.Invoke();
                }
            }
        }
    }
}
