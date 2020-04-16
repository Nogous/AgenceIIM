using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraHandler))]
public class CameraHandlerEditor : Editor
{
    /*private void OnSceneViewGUI(SceneView sv)
    {
        CameraHandler ch = target as CameraHandler;

        ch.positionDepart = Handles.PositionHandle(ch.positionDepart, Quaternion.identity);
        ch.positionAlternatif = Handles.PositionHandle(ch.positionAlternatif, Quaternion.identity);
        ch.startTangent = Handles.PositionHandle(ch.startTangent, Quaternion.identity);
        ch.endTangent = Handles.PositionHandle(ch.endTangent, Quaternion.identity);

        Handles.DrawBezier(ch.startPoint, ch.endPoint, ch.startTangent, ch.endTangent, Color.red, null, 2f);
    }*/

    void OnEnable()
    {
        Debug.Log("OnEnable");
        //SceneView.onSceneGUIDelegate += OnSceneViewGUI;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        //SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
    }
}

