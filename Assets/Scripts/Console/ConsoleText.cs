using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ConsoleText : MonoBehaviour {

    private TMPro.TextMeshProUGUI textMesh;

    private StringBuilder stringBuilder;

    void Start() {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void AddMessage (string message) {
        if (stringBuilder == null) stringBuilder = new StringBuilder();

        stringBuilder.Append(message + '\n');
    }

    public void UpdateText () {
        if (!textMesh) textMesh = GetComponent<TMPro.TextMeshProUGUI>();

        textMesh.text = stringBuilder.ToString();
        textMesh.autoSizeTextContainer = true;
    }

    //private void OnValidate () {
    //    textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    //    textMesh.autoSizeTextContainer = true;
    //}
}
