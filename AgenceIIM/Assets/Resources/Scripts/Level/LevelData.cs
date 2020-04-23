using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<CubeData> cubeDatas;

    public LevelData(Level level)
    {
        cubeDatas = level.cubeDatas;
    }
}