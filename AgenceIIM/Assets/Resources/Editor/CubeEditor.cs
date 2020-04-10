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
        color = myCube.color;


        renderer = ((Cube)target).GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sharedMaterial.color = color;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
