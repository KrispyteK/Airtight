using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager {

    public static bool allowInput = true;

    public static float GetAxis(string name) {
        if (!allowInput) return 0f;

        return Input.GetAxis(name);
    }

    public static float GetAxisRaw(string name) {
        if (!allowInput) return 0f;

        return Input.GetAxisRaw(name);
    }

    public static bool GetButtonDown(string name) {
        if (!allowInput) return false;

        return Input.GetButtonDown(name);
    }

    public static bool GetButtonUp(string name) {
        if (!allowInput) return false;

        return Input.GetButtonUp(name);
    }

    public static bool GetButton(string name) {
        if (!allowInput) return false;

        return Input.GetButton(name);
    }

    public static bool AnyKeyDown () {
        if (!allowInput) return false;

        return Input.anyKeyDown;
    }
}
