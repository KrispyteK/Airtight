using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable : MonoBehaviour {

    public bool toggled;

    public virtual void ToggleOn () {
        toggled = true;
    }

    public virtual void ToggleOff() {
        toggled = false;
    }

}
