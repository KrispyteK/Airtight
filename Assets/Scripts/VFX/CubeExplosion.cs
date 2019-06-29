using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplosion : MonoBehaviour {
    public GameObject effect;

    void OnDestroy () {
        Instantiate(effect, transform.position, transform.rotation);
    }
}
