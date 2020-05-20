using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    // data des niveau
    private int[] starsMonde1;
    private int[] starsMonde2;
    private int[] starsMonde3;

    // diferentes pages
    public GameObject mainMenuCanvas = null;
    public GameObject worldCanvas = null;
    public GameObject levelCanvas = null;

    // affichage info level
    public LevelUI[] levelUIMonde1;
    public LevelUI[] levelUIMonde2;
    public LevelUI[] levelUIMonde3;
    private int currentId = 0;
    private int currentWorld = 0;

    public LevelMenu[] levelObjMonde1;

    public Text nameLeveltext = null;
    public Text nbcouptext = null;
    public Text nbEnnemietext = null;
    public GameObject[] starsSelected = new GameObject[3];

    private void Awake()
    {
        OnClickMainMenu();

        starsMonde1 = new int[levelUIMonde1.Length];
        starsMonde2 = new int[levelUIMonde2.Length];
        starsMonde3 = new int[levelUIMonde3.Length];

        int index = 1;

        for (int i = 0; i < levelUIMonde1.Length; i++)
        {
            levelUIMonde1[i].idLevelText.text = index.ToString();
            levelUIMonde1[i].idLevel = index;
            index++;
        }
    }

    public void OnClickMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        worldCanvas.SetActive(false);
        levelCanvas.SetActive(false);
    }

    public void OnClickWorldSelection()
    {
        mainMenuCanvas.SetActive(false);
        worldCanvas.SetActive(true);
        levelCanvas.SetActive(false);
    }

    public void OnClickSelectWorld(int id)
    {
        currentWorld = id;
        mainMenuCanvas.SetActive(false);
        levelCanvas.SetActive(true);
        worldCanvas.SetActive(false);

        int[] currentList = new int[0];

        switch (id)
        {
            case 1:
                currentList = starsMonde1;
                break;
            case 2:
                currentList = starsMonde2;
                break;
            case 3:
                currentList = starsMonde3;
                break;
        }

        for (int i = 0; i < currentList.Length; i++)
        {
            if (levelUIMonde1[i].locker != null)
            {
                levelUIMonde1[i].locker.SetActive(true);
            }

            if (currentList[i] > 0)
            {
                if (levelUIMonde1[i].locker != null)
                {
                    levelUIMonde1[i].locker.SetActive(false);
                }

                levelUIMonde1[i].starUnlock1.SetActive(true);
                if (currentList[i] > 1)
                {
                    levelUIMonde1[i].starUnlock2.SetActive(true);
                    if (currentList[i] > 2)
                    {
                        levelUIMonde1[i].starUnlock3.SetActive(true);
                    }
                }
            }
        }

    }

    public void OnClickStartLevel()
    {
        SceneManager.LoadScene(currentWorld.ToString() + "-" + currentId.ToString());
    }

    private LevelMenu currentLevel = null;

    public void OnclikSelecteLevel(int i)
    {
        currentId = i-1;

        nameLeveltext.text = "Niveau : " + currentWorld.ToString() + "-" + currentId.ToString();


        LevelMenu[] currentList = levelObjMonde1;

        if (currentLevel != null)
        {
            if (currentLevel.cubes.Count > 0)
            {
                currentLevel.gameObject.SetActive(false);
            }
        }

        Debug.Log(currentList.Length);
        if (currentList.Length > currentId)
        {
            currentLevel = currentList[currentId];
            if (currentLevel != null)
            {
                currentLevel.gameObject.SetActive(true);
                currentLevel.LoadLevel();
            }
        }
    }
}
