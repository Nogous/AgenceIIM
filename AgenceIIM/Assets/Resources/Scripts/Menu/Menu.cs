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
    public levelUI[] levelMonde1;
    public levelUI[] levelMonde2;
    public levelUI[] levelMonde3;
    private int currentId = 0;
    private int currentWorld = 0;

    public LevelMenu[] levelsMonde1;
    public LevelMenu[] levelsMonde2;
    public LevelMenu[] levelsMonde3;

    public Text nameLeveltext = null;
    public Text nbcouptext = null;
    public Text nbEnnemietext = null;
    public GameObject[] starsSelected = new GameObject[3];

    private void Awake()
    {
        OnClickMainMenu();

        starsMonde1 = new int[levelMonde1.Length];
        starsMonde2 = new int[levelMonde2.Length];
        starsMonde3 = new int[levelMonde3.Length];

        int index = 1;

        for (int i = 0; i < levelMonde1.Length; i++)
        {
            levelMonde1[i].idLevelText.text = index.ToString();
            levelMonde1[i].idLevel = index;
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

    }

    public void OnClickStartLevel()
    {
        SceneManager.LoadScene(currentWorld.ToString() + "-" + currentId.ToString());
    }

    private LevelMenu currentLevel = null;

    public void OnclikSelecteLevel(int i)
    {
        currentId = i;

        nameLeveltext.text = "Niveau : " + currentWorld.ToString() + "-" + currentId.ToString();

        int[] currentList0;


        LevelMenu[] currentList;

        switch (currentWorld)
        {
            case 1:
                currentList = levelsMonde1;
                break;
            case 2:
                currentList = levelsMonde2;
                break;
            case 3:
                currentList = levelsMonde3;
                break;
            default:
                currentList = new LevelMenu[0];
                break;
        }

        if (currentLevel != null)
        {
            if (currentLevel.cubes.Count > 0)
            {
                currentLevel.cubes[0].transform.parent.gameObject.SetActive(false);
            }
        }

        Debug.Log(currentList.Length);
        if (currentList.Length >= i)
        {
            currentLevel = currentList[i - 1];
            if (currentLevel != null)
            {
                currentLevel.cubes[0].transform.parent.gameObject.SetActive(true);
                currentLevel.LoadLevel();
            }
        }
    }
}
