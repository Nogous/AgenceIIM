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
                        switch (level.cubesState[x, y])
                        {
                            case 0: // no cube
                                if (level.textureNull != null)
                                {
                                    buttonStyle.normal.background = level.textureNull;
                                }
                                break;
                            case 1: // baseCube
                                if (level.textureBase != null)
                                {
                                    buttonStyle.normal.background = level.textureBase;
                                }
                                break;
                            case 2: // color 1
                                if (level.textureColor1 != null)
                                {
                                    buttonStyle.normal.background = level.textureColor1;
                                }
                                break;
                            case 3: // Color 2
                                if (level.textureColor2 != null)
                                {
                                    buttonStyle.normal.background = level.textureColor2;
                                }
                                break;
                            case 4: // Color 3
                                if (level.textureColor3 != null)
                                {
                                    buttonStyle.normal.background = level.textureColor3;
                                }
                                break;
                        }

                        if (GUILayout.Button("", buttonStyle))
                        {
                            level.cubesState[x, y]++;
                            if (level.cubesState[x, y] > 4)
                            {
                                level.cubesState[x, y] = 0;
                            }

                            switch (level.cubesState[x, y])
                            {
                                case 0: // no cube
                                    level.cubes[x, y].gameObject.SetActive(false);
                                    level.cubes[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 1: // baseCube
                                    level.cubes[x, y].gameObject.SetActive(true);
                                    level.cubes[x, y].gameObject.GetComponent<Cube>().SetCubeBase(level.colorMat0);
                                    level.cubes[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 2: // color 1
                                    level.cubes[x, y].gameObject.SetActive(true);
                                    level.cubes[x, y].gameObject.GetComponent<Cube>().SetCubeColor(level.colorMat1);
                                    level.cubes[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 3: // Color 2
                                    level.cubes[x, y].gameObject.SetActive(true);
                                    level.cubes[x, y].gameObject.GetComponent<Cube>().SetCubeColor(level.colorMat2);
                                    level.cubes[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 4: // Color 3
                                    level.cubes[x, y].gameObject.SetActive(true);
                                    level.cubes[x, y].gameObject.GetComponent<Cube>().SetCubeColor(level.colorMat3);
                                    level.cubes[x, y].name = string.Format("{0},{1}", x, y);
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
                        switch (level.enemyState[x, y])
                        {
                            case 0: // no cube
                                if (GameManager.instance.texture2DNoCube != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DNoCube;
                                }
                                break;
                            case 1: // isEnemy
                                if (GameManager.instance.texture2DEnemyStatic != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DEnemyStatic;
                                }
                                break;
                            case 2: // isEnemyMoving
                                if (GameManager.instance.texture2DEnemyMove != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DEnemyMove;
                                }
                                break;
                            case 3: // isCleaningBox
                                if (GameManager.instance.texture2DCleaningBox != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DCleaningBox;
                                }
                                break;
                            case 4: // isDashBox
                                if (GameManager.instance.texture2DDashBox != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DDashBox;
                                }
                                break;
                            case 5: // isWall
                                if (GameManager.instance.texture2DWall != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DWall;
                                }
                                break;
                            case 6: // isTnt
                                if (GameManager.instance.texture2DTNT != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DTNT;
                                }
                                break;
                            case 7: // isTrigger
                                if (GameManager.instance.texture2DTrigger != null)
                                {
                                    buttonStyle.normal.background = GameManager.instance.texture2DTrigger;
                                }
                                break;
                        }

                        if (GUILayout.Button("", buttonStyle))
                        {
                            level.enemyState[x, y]++;
                            if (level.enemyState[x, y] > 7)
                            {
                                level.enemyState[x, y] = 0;
                            }

                            switch (level.enemyState[x, y])
                            {
                                case 0: // no cube
                                    level.enemys[x, y].gameObject.SetActive(false);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 1: // isEnemy
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeBase(GameManager.instance.matEnemy);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 2: // isEnemyMoving
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matEnemyMove);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 3: // isCleaningBox
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matCleaningBox);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 4: // isDashBox
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matDashBox);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 5: // isWall
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matWall);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 6: // isTnt
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matTNT);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
                                    break;
                                case 7: // isTrigger
                                    level.enemys[x, y].gameObject.SetActive(true);
                                    level.enemys[x, y].gameObject.GetComponent<Cube>().SetCubeColor(GameManager.instance.matTrigger);
                                    level.enemys[x, y].name = string.Format("{0},{1}", x, y);
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

            level.SetCubesState(level.cubesState);
        }


    }
}
