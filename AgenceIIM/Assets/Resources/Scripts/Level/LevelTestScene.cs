using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTestScene : MonoBehaviour
{
    public Level level;

    public void OnClickLoadLevel()
    {
        // destruction de tout les cubes
        for (int i = level.cubes.Count; i-->0;)
        {
            GameObject obj = level.cubes[i];
            level.cubes.Remove(obj);
            DestroyImmediate(obj);
        }

        // recuperation des data
        LevelData levelData = SaveSystem.LoadLevel();
        level.cubeDatas = levelData.cubeDatas;


        for (int j = level.cubeDatas.Count; j-->0;)
        {
            CubeData cd = level.cubeDatas[j];
            //level.SetupCube(cd.cubeType, new Vector3(cd.posX, cd.posY, cd.posZ));
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

        Debug.Log("spawn d'un item");

        CubeData cubeData = new CubeData();
        level.cubeDatas.Add(cubeData);

        level.SetupCube(cubeType, pos);
    }
}