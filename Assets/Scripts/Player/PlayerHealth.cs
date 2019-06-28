using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public Transform cameraTransform;
    public Volume sceneSettingsVolume;

    public bool isAlive = true;

    private UnityEngine.Experimental.Rendering.HDPipeline.Vignette vignette;
    private UnityEngine.Experimental.Rendering.HDPipeline.Vignette standardVignette;

    private void Awake() {
        sceneSettingsVolume = FindObjectOfType<Volume>();

        var hasVignette = sceneSettingsVolume.profile.TryGet(out vignette);

        standardVignette = Instantiate(vignette);
    }

    public void Update () {
        float normalizedHealth = health / 100f;

        vignette.color.Override(Color.Lerp(Color.red * 0.5f, standardVignette.color.value, normalizedHealth));
        vignette.intensity.Override(Mathf.Lerp(1f, standardVignette.intensity.value, normalizedHealth));
        vignette.smoothness.Override(Mathf.Lerp(1f, standardVignette.smoothness.value, normalizedHealth));

        if (!isAlive) {
            if (Input.anyKey) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void TakeDamage (int amount) {
        health = Mathf.Max(health - amount,0);

        if (health == 0) {
            Kill();
        }
    }

    public void Kill () {
        if (!isAlive) return;

        health = 0;
        isAlive = false;

        cameraTransform.GetComponent<PlayerCamera>().enabled = false;
        cameraTransform.GetComponent<GravityGun>().enabled = false;

        var col = cameraTransform.gameObject.AddComponent<SphereCollider>();

        col.radius = 0.25f;

        var rb = cameraTransform.gameObject.AddComponent<Rigidbody>();

        rb.velocity = gameObject.GetComponent<Movement>().velocity;

        //rb.AddForce(Vector3.down);
        //rb.AddTorque(Random.insideUnitSphere * 0.1f);

        foreach (var component in gameObject.GetComponents<MonoBehaviour>()) {
            if (component == this) continue;

            component.enabled = false;
        }

    }
}
