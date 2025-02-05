﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPartDoor : Toggleable {
    public Transform left;
    public Transform right;

    public float distance = 1.9f;
    public float speed = 0.5f;

    private float t;

    private void Update () {
        t = Mathf.Clamp(t + Time.deltaTime * speed * (toggled ? 1 : -1), 0, distance);

        left.localPosition = new Vector3(0,0,t);
        right.localPosition = new Vector3(0,0, -t);
    }
}
