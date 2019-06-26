using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour {
    public UnityEvent onInteract;
    public UnityEvent onStartInteract;
    public UnityEvent onEndInteract;
}
