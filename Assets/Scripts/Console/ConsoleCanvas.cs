using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCanvas : MonoBehaviour {

    public ScrollRect scrollRect; 
    public RectTransform scrollContent;
    public GameObject messagePrefab;
    public ConsoleText consoleText;
    public TMPro.TMP_InputField inputField;

    private void OnEnable () {
        inputField.Select();
        inputField.ActivateInputField();
        inputField.onSubmit.AddListener(OnInputFieldEnter);

        scrollRect.normalizedPosition = Vector2.zero;

        consoleText.UpdateText();
    }

    private void OnInputFieldEnter (string input) {
        if (string.IsNullOrEmpty(input)) return;
            
        Debug.Log(input);

        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();

        StartCoroutine(SetNormalizedPosition());
    }

    public void AddMessage(ConsoleMessage message) {
        var beforePosition = scrollRect.normalizedPosition;

        consoleText.AddMessage(message.condition);

        if (gameObject.activeSelf) consoleText.UpdateText();

        if (beforePosition == Vector2.zero && gameObject.activeSelf) {
            StartCoroutine(SetNormalizedPosition());
        }
    }

    IEnumerator SetNormalizedPosition () {
        yield return new WaitForEndOfFrame();

        scrollRect.normalizedPosition = Vector2.zero;
    }
}
