using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Menu))]
[CanEditMultipleObjects]
public class MenuEditor : Editor
{

    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset Score"))
        {
            SaveSystem.SavePoints(new int[0], "starsMonde1");
            SaveSystem.SavePoints(new int[0], "starsMonde2");
            SaveSystem.SavePoints(new int[0], "starsMonde3");
        }
    }
}
