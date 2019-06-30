using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : Singleton<Console> {

    public bool isOpened;

    public GameObject UI;
    public ConsoleSettings settings;

    public List<ConsoleMessage> messages = new List<ConsoleMessage>();

    [RuntimeInitializeOnLoadMethod]
    private static void StartOnSceneLoad() {
        var instance = Instance;
    }

    private void OnEnable() {
        settings = Resources.Load<ConsoleSettings>("Settings/Console Settings");

        Application.logMessageReceived += LogMessage;
    }

    private void Update() {
        if (!isOpened) {
            if (Input.GetKeyDown("`")) {
                OpenConsole();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseConsole();
            }
        }
    }

    public void OpenConsole() {
        isOpened = true;

        InputManager.allowInput = false;

        Cursor.lockState = CursorLockMode.None;

        CreateConsoleUI();

        UI.SetActive(true);
    }

    public void CloseConsole() {
        isOpened = false;

        InputManager.allowInput = true;

        Cursor.lockState = CursorLockMode.Locked;

        UI.SetActive(false);
    }

    public void CreateConsoleUI () {
        if (UI) return;

        UI = Instantiate(settings.UIPrefab);

        UI.GetComponent<Canvas>().worldCamera = Camera.main;

        DontDestroyOnLoad(UI);
    }

    public void LogMessage (string condition, string stackTrace, LogType type) {
        print(condition);

        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("Console") && type == LogType.Error) return;

        var msg = new ConsoleMessage {
            condition = condition,
            stackTrace = stackTrace,
            type = type
        };

        messages.Add(msg);

        UI.GetComponent<ConsoleCanvas>().AddMessage(msg);
    }
}
