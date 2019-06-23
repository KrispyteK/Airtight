using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEmitter : MonoBehaviour {

    public GameObject prefab;
    public Transform emitTransform;

    void Awake() {
        StartCoroutine(Emit());
    }

    private IEnumerator Emit () {
        while (true) {
            yield return new WaitForSeconds(0.05f);

            var instance = Instantiate(prefab, emitTransform.position, Quaternion.LookRotation(emitTransform.up * 10f + Random.insideUnitSphere));

            instance.GetComponent<AirProjectile>().velocityScalar = 5f;

            instance.GetComponent<Rigidbody>().velocity = emitTransform.up * 1f;

            print(instance.GetComponent<Rigidbody>().velocity);
        }
    }
}
