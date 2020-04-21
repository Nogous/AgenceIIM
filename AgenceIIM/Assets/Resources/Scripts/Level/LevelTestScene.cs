using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTestScene : MonoBehaviour
{
    public Level level;

    public void OnClickLoadLevel()
    {
        LevelData levelData = SaveSystem.LoadLevel();

        level.i = levelData.i;
        Debug.Log(levelData.i);
    }

    public void OnClickSaveLevel()
    {
        SaveSystem.SaveLevel(level);
    }
}
