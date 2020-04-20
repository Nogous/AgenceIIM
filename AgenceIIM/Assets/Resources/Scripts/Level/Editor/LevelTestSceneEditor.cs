using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelTestScene))]
public class LevelTestSceneEditor : Editor
{
    LevelTestScene level;

    private void OnEnable()
    {
        level = (LevelTestScene)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            level.OnClickLoadLevel();
        }
        if (GUILayout.Button("Save"))
        {
            level.OnClickSaveLevel();
        }
    }
}
