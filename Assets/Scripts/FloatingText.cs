using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Show()
    {

        var go = new GameObject("FloatingText");
        var floatingText = go.AddComponent<FloatingText>();
    }

    public void OnGUI()
    {

        GUI.Label(new Rect(100, 100, 50, 50), new GUIContent("Checkpoint!!!!"), Resources.Load<GUISkin>("Game GUISkin").GetStyle("CheckPointsText"));

    }
}
