using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject cube;
    public float timeBetweenUse = 2f;
    public GameObject instance;

    private float useTime = 0f;


    public void SpawnCube () {
        if ((Time.timeSinceLevelLoad - useTime) > timeBetweenUse) {
            Destroy(instance);

            useTime = Time.timeSinceLevelLoad;

            instance = Instantiate(cube, transform.position, transform.rotation);
        }
    }
}
