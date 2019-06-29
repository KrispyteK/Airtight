using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour {
    public bool isActive = true;

    public void SetActive (bool active) {
        isActive = active;
    }

    public UnityEvent onInteract;
    public UnityEvent onStartInteract;
    public UnityEvent onEndInteract;
}
