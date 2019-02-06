using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public GUISkin skin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {

            GUILayout.BeginVertical(skin.GetStyle("GameHud"));
            {

                GUILayout.Label(string.Format("Points: {0} ", GameMenager.Instance.point), skin.GetStyle("PointText"));

                var time = LevelManager.Instance.RunningTime;

                GUILayout.Label(string.Format("{0:00}:{1:00} with {2} bonus ", time.Minutes + (time.Hours * 60), time.Seconds, LevelManager.Instance.CurrentTimeBonus), skin.GetStyle("TimeText"));

            }
            GUILayout.EndVertical();


        } GUILayout.EndArea();
    }
}
