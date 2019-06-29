using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour {

    public Transform buttonTransform;
    public float pressDistance = 0.1f;
    public float speed = 10f;

    private Vector3 offset;
    private Button interactable;

    void Start() {
        interactable = GetComponent<Button>();

        offset = buttonTransform.localPosition;
    }

    void Update() {
        var distance = interactable.isPressed || !interactable.isActive ? pressDistance : 0;

        buttonTransform.localPosition = Vector3.Lerp(buttonTransform.localPosition, offset + Vector3.down * distance, Time.deltaTime * speed);
    }
}
