using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleCanvas : MonoBehaviour {

    public RectTransform scrollContent;
    public GameObject messagePrefab;
    public TMPro.TMP_InputField inputField;

    private void OnEnable () {
        inputField.onSubmit.AddListener(OnInputFieldEnter);
    }

    private void OnInputFieldEnter (string input) {
        Debug.Log(input);

        inputField.text = "";
    }

    public void AddMessage (ConsoleMessage message) {
        var instance = Instantiate(messagePrefab, scrollContent);

        instance.GetComponent<ConsoleMessageUI>().SetMessage(message.condition, message.type);
    }
}
