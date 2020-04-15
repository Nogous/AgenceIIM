using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    bool[,] cubes = null;
   //ameObject[,] cubesObj;
    private Dictionary<Vector2, GameObject> cubesObj = new Dictionary<Vector2, GameObject>();
    Vector2 saveSize;
    Level level;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        level = (Level)target;

        if (GUILayout.Button("Reset Level"))
        {
            Debug.Log("Level size set to : " + level.length + " x " + level.height);
            SetLevelSize(level.height, level.length);
        }

        if (GUILayout.Button("spawn"))
        {
            cubesObj.Add(new Vector2(0, 0), Instantiate(level.cubePrefab));
        }
        if (GUILayout.Button("reset dictionary"))
        {
            cubesObj.Clear();
        }

        for (int y = 0; y < saveSize.y; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < saveSize.x; x++)
            {
                if (saveSize.x >= x && saveSize.y >= y)
                {
                    GUILayout.Toggle(cubes[x, y], "");
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    public void SetLevelSize(int height, int length)
    {
        cubes = new bool[height,length];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                //Debug.Log(cubesObj.Count);
                GameObject toto =null;
                cubesObj.TryGetValue(new Vector2(x,y), out toto);
                if (toto != null)
                {
                    Debug.Log("weeeee");
                }
            }
        }
        saveSize = new Vector2(length, height);
    }

    public void GenerateLevel(int height, int length)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                /*
                if (cubesObj[x, y] == null && cubes[y,x])
                {
                    cubesObj[x, y] = level.CreateCube();
                    cubesObj[x, y].transform.position = new Vector3(-x + (length / 2), 0f, -y + (height / 2));
                }
                */
            }
        }
    }
}
