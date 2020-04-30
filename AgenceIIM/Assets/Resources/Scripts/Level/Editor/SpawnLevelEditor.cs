using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpawnLevel))]
public class SpawnLevelEditor : Editor
{
    SpawnLevel spawnLevel;

    private void OnEnable()
    {
        spawnLevel = (SpawnLevel)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            OnClickLoadLevel();
        }

        if (GUILayout.Button("UnLoad"))
        {
            OnClickUnLoadLevel();
        }
    }

    private void OnClickLoadLevel()
    {
        spawnLevel.StartSpawnLevel();
    }

    private void OnClickUnLoadLevel()
    {
        spawnLevel.StartUnPopLevel();
    }
}
