using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject cube;
    public bool canSpawn = true;
    public GameObject instance;

    public void SetActive (bool active) {
        canSpawn = active;
    }

    public void SpawnCube () {
        if (canSpawn) {
            if (instance) Destroy(instance);

            instance = Instantiate(cube, transform.position, transform.rotation);
        }
    }
}
