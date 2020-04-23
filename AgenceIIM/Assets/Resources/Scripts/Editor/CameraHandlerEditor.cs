using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraHandler))]
public class CameraHandlerEditor : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        SceneView.onSceneGUIDelegate += OnSceneViewGUI;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
    }
}

