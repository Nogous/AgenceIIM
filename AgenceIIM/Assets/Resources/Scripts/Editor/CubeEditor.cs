using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cube))]
[CanEditMultipleObjects]
public class CubeEditor : Editor
{
    SerializedProperty isEnemy;
    SerializedProperty isBreakable;
    Color color;

    private Renderer renderer;

    Cube myCube;

    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();


        //isEnemy = serializedObject.FindProperty("isEnemy");
        //isBreakable = serializedObject.FindProperty("isBreakable");

        //EditorGUILayout.PropertyField(isEnemy, new GUIContent("Is Enemy"));
        //EditorGUILayout.PropertyField(isBreakable, new GUIContent("IsBreakable"));
        EditorGUILayout.ColorField(color);

        myCube = (Cube)target;

        if (GUILayout.Button("Set Cube Base"))
        {
            if (GameManager.instance == null)
            {
                Debug.Log("Trigger GameManager svp");
                myCube.SetCubeBase();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
