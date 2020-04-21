using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private Level level;
    private SerializedObject soLevel;

    private SerializedProperty levelSize;
    private SerializedProperty cubePrefab;
    private SerializedProperty cubeType;
    private SerializedProperty player;

    string[] tabs = null;
    public int tabTarget = 0;

    private void OnEnable()
    {
        level = (Level)target;
        soLevel = new SerializedObject(target);

        levelSize = soLevel.FindProperty("levelSize");
        cubePrefab = soLevel.FindProperty("cubePrefab");
        cubeType = soLevel.FindProperty("cubeType");
        player = soLevel.FindProperty("player");

        tabs = new string[] { "Player", "Level 0", "Level 1", "Option" };
    }

    public override void OnInspectorGUI()
    {
        soLevel.Update();
        EditorGUI.BeginChangeCheck();
        tabTarget = GUILayout.Toolbar(tabTarget, tabs);

        switch (tabTarget)
        {
            case 3:
                #region case 3

                EditorGUILayout.PropertyField(levelSize);
                EditorGUILayout.PropertyField(cubePrefab);

                if (cubePrefab != null)
                {
                    if (GUILayout.Button("Load"))
                    {
                        OnClickLoadLevel();
                    }
                    if (GUILayout.Button("Save"))
                    {
                        OnClickSaveLevel();
                    }
                    if (GUILayout.Button("Reset"))
                    {
                        OnClickReseLevelt();
                    }

                    if (GUILayout.Button("SpawnCube000"))
                    {
                        OnClickSpawnCube(CubeType.Base, Vector3.zero);
                    }
                }
                #endregion
                break;
            case 1:
                #region case 1

                EditorGUILayout.PropertyField(cubeType);

                if (level.levelSize.x > 0 && level.levelSize.y > 0)
                {
                    for (int x = 0; x < level.levelSize.x; x++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int y = 0; y < level.levelSize.y; y++)
                        {
                            if (GUILayout.Button(""))
                            {
                                OnClickSpawnCube(level.cubeType, new Vector3(y - level.levelSize.y / 2, 0, -x + level.levelSize.x / 2));
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                #endregion
                break;
            case 2:
                #region case 2

                EditorGUILayout.PropertyField(cubeType);

                if (level.levelSize.x > 0 && level.levelSize.y > 0)
                {
                    for (int x = 0; x < level.levelSize.x; x++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int y = 0; y < level.levelSize.y; y++)
                        {
                            if (GUILayout.Button(""))
                            {
                                OnClickSpawnCube(level.cubeType, new Vector3(y - level.levelSize.y / 2, 1, -x + level.levelSize.x / 2));
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                #endregion
                break;
            case 0:
                #region case 0

                EditorGUILayout.PropertyField(player);

                if (level.levelSize.x > 0 && level.levelSize.y > 0)
                {
                    for (int x = 0; x < level.levelSize.x; x++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int y = 0; y < level.levelSize.y; y++)
                        {
                            if (GUILayout.Button(""))
                            {
                                level.player.transform.position = new Vector3(y - level.levelSize.y / 2, 1, -x + level.levelSize.x / 2);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                #endregion
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soLevel.ApplyModifiedProperties();
        }
    }


    public void OnClickLoadLevel()
    {
        // destruction de tout les cubes
        for (int i = level.cubes.Count; i-- > 0;)
        {
            GameObject obj = level.cubes[i];
            level.cubes.Remove(obj);
            DestroyImmediate(obj);
        }

        // recuperation des data
        level.cubeDatas = SaveSystem.LoadLevel().cubeDatas;

        for (int j = level.cubeDatas.Count; j-- > 0;)
        {
            CubeData cd = level.cubeDatas[j];
            level.SetupCube(cd.cubeType, new Vector3(cd.posX, cd.posY, cd.posZ));
        }
    }

    public void OnClickSaveLevel()
    {
        SaveSystem.SaveLevel(level);
    }

    public void OnClickReseLevelt()
    {
        level.cubeDatas = new List<CubeData>();

        for (int i = level.cubes.Count; i-- > 0;)
        {
            GameObject obj = level.cubes[i];
            level.cubes.Remove(obj);
            DestroyImmediate(obj);
        }

        //SaveSystem.SaveLevel(level);
    }

    public void OnClickSpawnCube(CubeType cubeType, Vector3 pos)
    {
        for (int i = level.cubeDatas.Count; i-- > 0;)
        {
            if (level.cubeDatas[i].id == pos.ToString())
            {
                level.SetupCube(cubeType, pos);
                return;
            }
        }
        CubeData cubeData = new CubeData();
        cubeData.id = pos.ToString();
        cubeData.cubeType = cubeType;
        cubeData.posX = pos.x;
        cubeData.posY = pos.y;
        cubeData.posZ = pos.z;
        level.cubeDatas.Add(cubeData);

        level.SetupCube(cubeType, pos);
    }
}
