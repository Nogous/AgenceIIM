using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LevelInfo
{
    public GameObject star0;
    public GameObject star1;
    public GameObject star2;

    public GameObject starUnlock0;
    public GameObject starUnlock1;
    public GameObject starUnlock2;

    public LevelMenu level = null;
}

[System.Serializable]
public class WorldInfo
{
    public GameObject worldCanvas;
    public List<LevelInfo> levels;

    public Text nameLeveltext = null;
    public Text nbcouptext = null;
    public Text nbEnnemietext = null;
}

public class Menu : MonoBehaviour
{
    public GameObject mainMenuCanvas = null;
    public GameObject worldChoseCanvas = null;
    public GameObject levelhoseCanvas = null;

    public int nbLevel = 11;
    private int currentId = 0;
    private int currentMonde = 0;

    // affichage info level
    public List<WorldInfo> mondes;

    public void OnClickMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        worldChoseCanvas.SetActive(false);
        levelhoseCanvas.SetActive(false);
    }

    public void OnClickWorldSelection()
    {
        mainMenuCanvas.SetActive(false);
        worldChoseCanvas.SetActive(true);
        levelhoseCanvas.SetActive(false);
    }

    public void OnClickSelectWorld(int id)
    {
        currentMonde = id;
        levelhoseCanvas.SetActive(true);
        worldChoseCanvas.SetActive(false);
        for (int i = mondes.Count; i -->0;)
        {
            mondes[i].worldCanvas.SetActive(false);
        }
        mondes[id].worldCanvas.SetActive(true);
    }

    public void OnClickStartLevel()
    {
        SceneManager.LoadScene("Level " + currentId + ".1");
    }

    public void OnclikSelecteLevel(int i)
    {
        Debug.Log("click " + i);
        currentId = i;
        /*
        if (currentId >= nbLevel)
        {
            currentId -= nbLevel;
        }
        if (currentId < 0)
        {
            currentId = nbLevel + i;
        }
        */
        //mondes[currentMonde].nameLeveltext.text = "Level "+ currentMonde+"-" + currentId;
        if (mondes[currentMonde].levels[i].level != null)
        {
            mondes[currentMonde].levels[i].level.LoadLevel();
        }
    }

    public void SpawnLevelUI(int id)
    {

    }
}
