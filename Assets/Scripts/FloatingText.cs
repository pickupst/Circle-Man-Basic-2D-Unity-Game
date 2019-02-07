using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private GUIContent _content;
    private static readonly GUISkin skin = Resources.Load<GUISkin>("Game GUISkin");

    public GUIStyle Style { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Show(string text, string style)
    {

        var go = new GameObject("FloatingText");
        var floatingText = go.AddComponent<FloatingText>();

        floatingText._content = new GUIContent(text);
        floatingText.Style = skin.GetStyle(style);
    }

    public void OnGUI()
    {

        GUI.Label(new Rect(100, 100, 50, 50), _content, Style);

    }
}
