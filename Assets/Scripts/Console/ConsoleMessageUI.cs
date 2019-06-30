using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMessageUI : MonoBehaviour {
    private TMPro.TextMeshProUGUI textMesh;

    public void SetMessage (string text, LogType type) {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.text = text;

        int returns = text.Split('\n').Length - 1;

        print(returns);

        float height = textMesh.fontSize * Mathf.Max(returns, 1);

        GetComponent<LayoutElement>().minHeight = height;

        //textMesh.autoSizeTextContainer = true;

        switch (type) {
            case LogType.Error:
                textMesh.color = Color.red;
                break;
            case LogType.Warning:
                textMesh.color = Color.yellow;
                break;
            default:
                textMesh.color = Color.white;
                break;
        }
    }
}
