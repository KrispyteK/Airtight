using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleUI : MonoBehaviour {
    private Console console;
    private Vector2 scrollPosition = Vector2.zero;
    private string inputText = "";

    private void Awake() {
        console = GetComponent<Console>();
    }

    private void Update () {
        if (Input.GetKeyDown("enter") && GUI.GetNameOfFocusedControl() == "InputField") {
            print(inputText);

            inputText = "";
        }
    }

    private void OnGUI() {
        if (!console.isOpened) return;

        GUI.Box(new Rect(0, 0, Screen.width, 500), "Console");

        // An absolute-positioned example: We make a scrollview that has a really large client
        // rect and put it in a small rect on the screen.
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width, 500), scrollPosition, new Rect(0, 0, Screen.width, 500));

        float y = 0;

        for (int i = 0; i < console.messages.Count; i++) {
            DrawMessage(console.messages[i], ref y);
        }

        GUI.EndScrollView();

        GUI.SetNextControlName("InputField");
        inputText = GUI.TextField(new Rect(10, 500 - 25, Screen.width - 20, 20), inputText);

        //GUI.FocusControl("InputField");
    }

    private void DrawMessage (ConsoleMessage message, ref float y) {
        float height = console.settings.guiStyle.CalcHeight(new GUIContent(message.condition), Screen.width - 20);

        GUI.Box(new Rect(0, y, Screen.width, height), message.condition);

        y += height;
    }
}
