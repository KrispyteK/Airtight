using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable {
    public bool isPressed;

    private void Start () {
        onStartInteract.AddListener(() => {
            if (!isActive) return;

            isPressed = true;
        });

        onEndInteract.AddListener(() => {
            if (!isActive) return;

            isPressed = false;
        });
    }
}
