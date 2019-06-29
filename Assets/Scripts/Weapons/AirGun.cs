using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirGun : MonoBehaviour {

    public float fireDelay;
    public GameObject prefab;

    private float _lastFireTime;

    void Start() {
        _lastFireTime = -fireDelay;
    }

    void Update() {
        if (InputManager.GetButton("Fire1") && (Time.timeSinceLevelLoad - _lastFireTime) > fireDelay) {
            _lastFireTime = Time.timeSinceLevelLoad;

            var instance = Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}
