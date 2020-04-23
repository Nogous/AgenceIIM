using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PlaytestAnalitic))]
public class PlaytestAnaliticEditor : Editor
{
    PlaytestAnalitic analitic;

    private void OnEnable()
    {
        analitic = (PlaytestAnalitic)target;

        analitic.nbDeath = new int[analitic.nbLevelToTest];
        analitic.timeDuration = new float[analitic.nbLevelToTest];
        analitic.nbMoveCam = new int[analitic.nbLevelToTest];

        analitic.ShowData();
        analitic.data.text = "DATA";
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("ShowData"))
        {
            analitic.ShowData();
        }
        if (GUILayout.Button("HideData"))
        {
            analitic.HideData();
        }
    }
}
