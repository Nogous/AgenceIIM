using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[System.Serializable]
enum ColorTile
{

}

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level level;
    public GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);

    //GUIStyle foldoutColorStyle = new GUIStyle(EditorStyles.foldout);
    //foldoutColorStyle.normal.textColor = new color

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        level = (Level)target;

        if (GUILayout.Button("GenerateLevel"))
        {
            level.GenerateLevel();
        }

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

                    switch (level.cubesState[x,y])
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
    }
}
