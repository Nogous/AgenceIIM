using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private Level level;
    private SerializedObject soLevel;

    private SerializedProperty nameLevel;
    private SerializedProperty levelSize;
    private SerializedProperty cubePrefab;
    private SerializedProperty cubeType;
    private SerializedProperty player;

    string[] tabs = null;
    public int tabTarget = 0;


    public GUIStyle buttonStyle = new GUIStyle();

    private void OnEnable()
    {
        level = (Level)target;
        soLevel = new SerializedObject(target);

        nameLevel = soLevel.FindProperty("nameLevel");
        levelSize = soLevel.FindProperty("levelSize");
        cubePrefab = soLevel.FindProperty("cubePrefab");
        cubeType = soLevel.FindProperty("cubeType");
        player = soLevel.FindProperty("player");

        tabs = new string[] { "Player", "Level 0", "Level 1", "Option" };

        OnClickLoadLevel(true);
    }

    public override void OnInspectorGUI()
    {
        soLevel.Update();
        EditorGUI.BeginChangeCheck();
        tabTarget = GUILayout.Toolbar(tabTarget, tabs);

        switch (tabTarget)
        {
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
                            if (level.texture2DNoCube != null)
                            {
                                buttonStyle.normal.background = level.texture2DNoCube;
                            }
                            for (int i = level.cubeDatas.Count; i-->0;)
                            {
                                if (level.cubeDatas[i].id == new Vector3(y - level.levelSize.y / 2, level.transform.position.y, -x + level.levelSize.x / 2).ToString())
                                {

                                    switch (level.cubeDatas[i].cubeType)
                                    {
                                        case CubeType.NoCube:
                                            if (level.texture2DNoCube != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DNoCube;
                                            }
                                            break;
                                        case CubeType.Base:
                                            if (level.texture2DCubeBase != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeBase;
                                            }
                                            break;
                                        case CubeType.EnnemiStatique:
                                            if (level.texture2DEnnemiStatique != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiStatique;
                                            }
                                            break;
                                        case CubeType.EnnemiPattern:
                                            if (level.texture2DEnnemiPattern != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiPattern;
                                            }
                                            break;
                                        case CubeType.EnnemiMiroir:
                                            if (level.texture2DEnnemiMiroir != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiMiroir;
                                            }
                                            break;
                                        case CubeType.Peinture:
                                            if (level.texture2DCubePeinture != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubePeinture;
                                            }
                                            break;
                                        case CubeType.Cleaner:
                                            if (level.texture2DCubeCleaner != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeCleaner;
                                            }
                                            break;
                                        case CubeType.ArcEnCiel:
                                            if (level.texture2DCubeArcEnCiel != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeArcEnCiel;
                                            }
                                            break;
                                        case CubeType.Téléporteur:
                                            if (level.texture2DCubeTeleporteur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeTeleporteur;
                                            }
                                            break;
                                        case CubeType.Dash:
                                            if (level.texture2DCubeDash != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeDash;
                                            }
                                            break;
                                        case CubeType.Glissant:
                                            if (level.texture2DCubeGlissant != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeGlissant;
                                            }
                                            break;
                                        case CubeType.Mur:
                                            if (level.texture2DCubeMur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeMur;
                                            }
                                            break;
                                        case CubeType.TNT:
                                            if (level.texture2DCubeTNT != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeTNT;
                                            }
                                            break;
                                        case CubeType.Interrupteur:
                                            if (level.texture2DCubeInterrupteur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeInterrupteur;
                                            }
                                            break;
                                        case CubeType.Destructible:
                                            if (level.texture2DCubeDestructible != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeDestructible;
                                            }
                                            break;
                                        case CubeType.BlocMouvant:
                                            if (level.texture2DCubeBlocMouvant != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeBlocMouvant;
                                            }
                                            break;
                                        default:
                                            if (level.texture2DNoCube != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DNoCube;
                                            }
                                            break;
                                    }
                                    break;
                                }
                            }

                            if (GUILayout.Button("", buttonStyle))
                            {
                                OnClickSpawnCube(level.cubeType, new Vector3(y - level.levelSize.y / 2, level.transform.position.y, -x + level.levelSize.x / 2));
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
                            for (int i = level.cubeDatas.Count; i-- > 0;)
                            {
                                if (level.texture2DNoCube != null)
                                {
                                    buttonStyle.normal.background = level.texture2DNoCube;
                                }
                                if (level.cubeDatas[i].id == new Vector3(y - level.levelSize.y / 2, 1 + level.transform.position.y, -x + level.levelSize.x / 2).ToString())
                                {

                                    switch (level.cubeDatas[i].cubeType)
                                    {
                                        case CubeType.NoCube:
                                            if (level.texture2DNoCube != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DNoCube;
                                            }
                                            break;
                                        case CubeType.Base:
                                            if (level.texture2DCubeBase != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeBase;
                                            }
                                            break;
                                        case CubeType.EnnemiStatique:
                                            if (level.texture2DEnnemiStatique != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiStatique;
                                            }
                                            break;
                                        case CubeType.EnnemiPattern:
                                            if (level.texture2DEnnemiPattern != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiPattern;
                                            }
                                            break;
                                        case CubeType.EnnemiMiroir:
                                            if (level.texture2DEnnemiMiroir != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DEnnemiMiroir;
                                            }
                                            break;
                                        case CubeType.Peinture:
                                            if (level.texture2DCubePeinture != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubePeinture;
                                            }
                                            break;
                                        case CubeType.Cleaner:
                                            if (level.texture2DCubeCleaner != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeCleaner;
                                            }
                                            break;
                                        case CubeType.ArcEnCiel:
                                            if (level.texture2DCubeArcEnCiel != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeArcEnCiel;
                                            }
                                            break;
                                        case CubeType.Téléporteur:
                                            if (level.texture2DCubeTeleporteur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeTeleporteur;
                                            }
                                            break;
                                        case CubeType.Dash:
                                            if (level.texture2DCubeDash != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeDash;
                                            }
                                            break;
                                        case CubeType.Glissant:
                                            if (level.texture2DCubeGlissant != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeGlissant;
                                            }
                                            break;
                                        case CubeType.Mur:
                                            if (level.texture2DCubeMur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeMur;
                                            }
                                            break;
                                        case CubeType.TNT:
                                            if (level.texture2DCubeTNT != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeTNT;
                                            }
                                            break;
                                        case CubeType.Interrupteur:
                                            if (level.texture2DCubeInterrupteur != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeInterrupteur;
                                            }
                                            break;
                                        case CubeType.Destructible:
                                            if (level.texture2DCubeDestructible != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeDestructible;
                                            }
                                            break;
                                        case CubeType.BlocMouvant:
                                            if (level.texture2DCubeBlocMouvant != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DCubeBlocMouvant;
                                            }
                                            break;
                                        default:
                                            if (level.texture2DNoCube != null)
                                            {
                                                buttonStyle.normal.background = level.texture2DNoCube;
                                            }
                                            break;
                                    }
                                    break;
                                }
                            }

                            if (GUILayout.Button("", buttonStyle))
                            {
                                OnClickSpawnCube(level.cubeType, new Vector3(y - level.levelSize.y / 2, 1 + level.transform.position.y, -x + level.levelSize.x / 2));
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                #endregion
                break;
            case 3:
                #region case 3

                EditorGUILayout.PropertyField(nameLevel);

                EditorGUILayout.PropertyField(levelSize);
                EditorGUILayout.PropertyField(cubePrefab);

                if (cubePrefab != null)
                {
                    if (GUILayout.Button("Load"))
                    {
                        OnClickLoadLevel(false);
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

                base.DrawDefaultInspector();

                #endregion
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soLevel.ApplyModifiedProperties();
        }
    }


    public void OnClickLoadLevel(bool _isEditor)
    {
        if (level.cubeBox != null)
        {
            for (int i = level.cubeBox.childCount; i-->0;)
            {
                DestroyImmediate(level.cubeBox.GetChild(0).gameObject);
            }
        }
        else
        {
            if (level.transform.Find("CubeBox"))
            {
                level.cubeBox = level.transform.Find("CubeBox");
            }
            else
            {
                Debug.Log("cree un gameObject vide du nom de \"CubeBox\"");
            }
        }

        // destruction de tout les cubes
        for (int i = level.cubes.Count; i-- > 0;)
        {
            GameObject obj = level.cubes[i];
            level.cubes.Remove(obj);
            DestroyImmediate(obj);
        }

        // recuperation des data
        if (_isEditor)
        {
            level.cubeDatas = SaveSystem.LoadLevel("saveEditor").cubeDatas;
        }
        else
        {
            level.cubeDatas = SaveSystem.LoadLevel(level.nameLevel).cubeDatas;
        }

        for (int j = level.cubeDatas.Count; j-- > 0;)
        {
            CubeData cd = level.cubeDatas[j];
            level.SetupCube(cd.cubeType, new Vector3(cd.posX, cd.posY, cd.posZ));
        }

        SaveSystem.SaveLevel(level, "saveEditor");
    }

    public void OnClickSaveLevel()
    {
        SaveSystem.SaveLevel(level);
        SaveSystem.SaveLevel(level, "saveEditor");
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

        SaveSystem.SaveLevel(level, "saveEditor");
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

        SaveSystem.SaveLevel(level, "saveEditor");
    }
}
