using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {

    public UnityEvent onFinish;

    public void StartTimer (float seconds) {
        StartCoroutine(TimerRoutine(seconds));
    }

    private IEnumerator TimerRoutine (float seconds) {
        yield return new WaitForSeconds(seconds);

        onFinish.Invoke();
    }
}
