using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfterDelay : MonoBehaviour {

    public float time = 1f;
    public UnityEvent action;

    void Awake() {
        StartCoroutine(Routine());
    }

    IEnumerator Routine () {
        yield return new WaitForSeconds(time);

        action.Invoke();
    }

    public void DestroyGameObject () {
        Destroy(gameObject);
    }
}
