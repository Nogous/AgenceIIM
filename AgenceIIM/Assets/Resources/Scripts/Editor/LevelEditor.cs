using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
enum ColorTile
{

}

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private Level level;
    private SerializedObject soTarget;
    private SerializedProperty cubesState;

    string[] tabs = null;
    public GUIStyle buttonStyle = new GUIStyle();

    //GUIStyle foldoutColorStyle = new GUIStyle(EditorStyles.foldout);
    //foldoutColorStyle.normal.textColor = new color

    private void OnEnable()
    {
        level = (Level)target;
        soTarget = new SerializedObject(target);

        cubesState = soTarget.FindProperty("cubesState");

        tabs = new string[] { "Option", "Ground", "Enemy", "Player" };

    }

    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        level.tabTarget = GUILayout.Toolbar(level.tabTarget, tabs);

        switch (level.tabTarget)
        {
            case 0:
                base.OnInspectorGUI();
                break;
            case 1:
                if (GUILayout.Button("Reset level"))
                {
                    level.GenerateLevel();
                }

                // ground level
                for (int x = 0; x < level.levelSize.x; x++)
                {
                    GUILayout.BeginHorizontal();
                    for (int y = 0; y < level.levelSize.y; y++)
                    {
                        switch (level.cubesState[x + (y * (int)level.levelSize.x)])
                        {
                            case 1: // baseCube
                                if (level.texture2DCubeBase != null)
                                {
                                    buttonStyle.normal.background = level.texture2DCubeBase;
                                }
                                break;
                            case 2: // color 1
                                if (level.texture2DGrond1 != null)
                                {
                                    buttonStyle.normal.background = level.texture2DGrond1;
                                }
                                break;
                            case 3: // Color 2
                                if (level.texture2DGrond2 != null)
                                {
                                    buttonStyle.normal.background = level.texture2DGrond2;
                                }
                                break;
                            case 4: // Color 3
                                if (level.texture2DGrond3 != null)
                                {
                                    buttonStyle.normal.background = level.texture2DGrond3;
                                }
                                break;
                        }

                        if (GUILayout.Button("", buttonStyle))
                        {
                            level.cubesState[x + (y * (int)level.levelSize.x)]++;
                            if (level.cubesState[x + (y * (int)level.levelSize.x)] > 4)
                            {
                                level.cubesState[x + (y * (int)level.levelSize.x)] = 0;
                            }

                            switch (level.cubesState[x + (y * (int)level.levelSize.x)])
                            {
                                case 0: // no cube
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.SetActive(false);
                                    level.cubes[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 1: // baseCube
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetCubeBase(level.matCubeBase);
                                    level.cubes[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 2: // color 1
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetCubeColor(level.matColor1);
                                    level.cubes[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 3: // Color 2
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetCubeColor(level.matColor2);
                                    level.cubes[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 4: // Color 3
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.cubes[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetCubeColor(level.matColor3);
                                    level.cubes[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 2:

                if (GUILayout.Button("Reset level"))
                {
                    level.GenerateLevel();
                }

                // level 1
                for (int x = 0; x < level.levelSize.x; x++)
                {
                    GUILayout.BeginHorizontal();
                    for (int y = 0; y < level.levelSize.y; y++)
                    {
                        switch (level.enemyState[x + (y * (int)level.levelSize.x)])
                        {
                            case 0: // no cube
                                if (level.texture2DNoCube != null)
                                {
                                    buttonStyle.normal.background = level.texture2DNoCube;
                                }
                                break;
                            case 1: // isEnemy
                                if (level.texture2DEnemyStatic != null)
                                {
                                    buttonStyle.normal.background = level.texture2DEnemyStatic;
                                }
                                break;
                            case 2: // isEnemyMoving
                                if (level.texture2DEnemyMove != null)
                                {
                                    buttonStyle.normal.background = level.texture2DEnemyMove;
                                }
                                break;
                            case 3: // isCleaningBox
                                if (level.texture2DCleaningBox != null)
                                {
                                    buttonStyle.normal.background = level.texture2DCleaningBox;
                                }
                                break;
                            case 4: // isDashBox
                                if (level.texture2DDashBox != null)
                                {
                                    buttonStyle.normal.background = level.texture2DDashBox;
                                }
                                break;
                            case 5: // isWall
                                if (level.texture2DWall != null)
                                {
                                    buttonStyle.normal.background = level.texture2DWall;
                                }
                                break;
                            case 6: // isTnt
                                if (level.texture2DTNT != null)
                                {
                                    buttonStyle.normal.background = level.texture2DTNT;
                                }
                                break;
                            case 7: // isTrigger
                                if (level.texture2DTrigger != null)
                                {
                                    buttonStyle.normal.background = level.texture2DTrigger;
                                }
                                break;
                        }

                        if (GUILayout.Button("", buttonStyle))
                        {
                            level.enemyState[x + (y * (int)level.levelSize.x)]++;
                            if (level.enemyState[x + (y * (int)level.levelSize.x)] > 7)
                            {
                                level.enemyState[x + (y * (int)level.levelSize.x)] = 0;
                            }

                            switch (level.enemyState[x + (y * (int)level.levelSize.x)])
                            {
                                case 0: // no cube
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(false);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 1: // isEnemy
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetEnemy(level.matEnemy);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 2: // isEnemyMoving
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetEnemyMoving(level.matEnemyMove);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 3: // isCleaningBox
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetCleaningBox(level.matCleaningBox);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 4: // isDashBox
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetDashBox(level.matDashBox);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 5: // isWall
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetWall(level.matWall);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 6: // isTnt
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetTNT(level.matTNT);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 7: // isTrigger
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.SetActive(true);
                                    level.enemys[x + (y * (int)level.levelSize.x)].gameObject.GetComponent<Cube>().SetTrigger(level.matTrigger);
                                    level.enemys[x + (y * (int)level.levelSize.x)].name = string.Format("{0},{1}", x, y);
                                    break;
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                break;
            case 3:
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }


    }
}
