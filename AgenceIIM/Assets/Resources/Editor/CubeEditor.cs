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

    Renderer renderer;

    void OnEnable()
    {
        isEnemy = serializedObject.FindProperty("isEnemy");
        isBreakable = serializedObject.FindProperty("isBreakable");
        color = serializedObject.FindProperty("color").colorValue;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(isEnemy, new GUIContent("Is Enemy"));
        EditorGUILayout.PropertyField(isBreakable, new GUIContent("IsBreakable"));
        EditorGUILayout.ColorField(color);

        //EditorGUILayout.

        serializedObject.ApplyModifiedProperties();
    }

    void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (renderer != null)
            {
                renderer.sharedMaterial.color = color;
            }
        }
    }
}
