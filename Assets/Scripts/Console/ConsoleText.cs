using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public class ConsoleText : MonoBehaviour {

    private TMPro.TextMeshProUGUI textMesh;
    private StringBuilder stringBuilder;
    public int lines;

    void Start() {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void AddMessage (string message) {
        if (stringBuilder == null) stringBuilder = new StringBuilder();

        string str = System.Convert.ToString(stringBuilder);
        int numLines = str.Split('\n').Length;

        if (numLines > 250) {
            var firstLine = stringBuilder.ToString().IndexOf(System.Environment.NewLine, System.StringComparison.Ordinal);

            stringBuilder.Remove(0, firstLine + System.Environment.NewLine.Length);
        }

        stringBuilder.Append(message + System.Environment.NewLine);
    }

    public void UpdateText () {
        if (!textMesh) textMesh = GetComponent<TMPro.TextMeshProUGUI>();

        textMesh.text = stringBuilder?.ToString();
        textMesh.autoSizeTextContainer = true;
    }

    //private void OnValidate () {
    //    textMesh = GetComponent<TMPro.TextMeshProUGUI>();
    //    textMesh.autoSizeTextContainer = true;
    //}
}
