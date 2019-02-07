using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private GUIContent _content;
    private static readonly GUISkin skin = Resources.Load<GUISkin>("Game GUISkin");
    private IFloatingTextPositioner _positioner;

    public GUIStyle Style { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Show(string text, string style, IFloatingTextPositioner positioner)
    {

        var go = new GameObject("FloatingText");
        var floatingText = go.AddComponent<FloatingText>();

        floatingText._content = new GUIContent(text);
        floatingText.Style = skin.GetStyle(style);

        floatingText._positioner = positioner;
    }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(_content);

        if (!_positioner.GetPosition(ref position, _content, contentSize))
        {
            Destroy(gameObject);
            return;
        }



        GUI.Label(new Rect(position, contentSize), _content, Style);

    }
}
