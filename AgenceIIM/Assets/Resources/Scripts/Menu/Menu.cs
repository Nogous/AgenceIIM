using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public int nbLevel = 11;
    private int currentId = 0;

    // affichage info level
    public Text nameLeveltext = null;
    public Text nbcouptext = null;
    public Text nbEnnemietext = null;

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
        nameLeveltext.text = "Level 1-" + currentId;
    }
}
