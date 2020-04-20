using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int i;

    public LevelData(Level level)
    {
        i = level.i;
    }
}