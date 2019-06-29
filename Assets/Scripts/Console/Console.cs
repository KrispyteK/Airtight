using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : Singleton<Console> {

    public bool isOpened;

    public ConsoleSettings settings;

    public GameObject uiElement;

    public RectTransform canvas;

    [RuntimeInitializeOnLoadMethod]
    private static void StartOnSceneLoad () {
        var instance = Instance;
    }

    private void Start () {
        settings = Resources.Load<ConsoleSettings>("Settings/Console Settings");

        canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<RectTransform>();
    }

    private void Update () {
        if (!isOpened) {
            if (Input.GetKeyDown("`")) {
                OpenConsole();
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseConsole();
            }
        }
    }

    public void OpenConsole () {
        isOpened = true;

        InputManager.allowInput = false;

        Cursor.lockState = CursorLockMode.None;

        if (!uiElement) {
            uiElement = Instantiate(settings.UIPrefab, canvas);
        } else {
            uiElement.SetActive(true);
        }
    }
    public void CloseConsole() {
        isOpened = false;

        InputManager.allowInput = true;

        Cursor.lockState = CursorLockMode.Locked;

        uiElement.SetActive(false);
    }
}
