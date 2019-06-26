using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Interactable {
    void OnTriggerEnter (Collider collider) {
        onStartInteract.Invoke();
    }

    void OnTriggerExit(Collider collider) {
        onEndInteract.Invoke();
    }
}
