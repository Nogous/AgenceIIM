using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraHandler))]
public class CameraHandlerEditor : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
        CameraHandler ch = target as CameraHandler;

        /*ch.positionDepart = Handles.PositionHandle(ch.positionDepart, Quaternion.identity);
        ch.positionAlternatif = Handles.PositionHandle(ch.positionAlternatif, Quaternion.identity);*/
        ch.TanDepart = Handles.PositionHandle(ch.TanDepart, Quaternion.identity);
        ch.TanAlternatif = Handles.PositionHandle(ch.TanAlternatif, Quaternion.identity);

        Handles.DrawBezier(ch.positionDepart, ch.positionAlternatif, ch.TanDepart, ch.TanAlternatif, Color.red, null, 2f);
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

