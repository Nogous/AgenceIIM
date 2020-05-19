using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class Stars
{
    public GameObject starUnlock1;
    public GameObject starUnlock2;
    public GameObject starUnlock3;
}

public class Menu : MonoBehaviour
{
    // data des niveau
    private int[] starsMonde1 = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] starsMonde2 = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] starsMonde3 = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // diferentes pages
    public GameObject mainMenuCanvas = null;
    public GameObject worldCanvas = null;
    public GameObject levelCanvas = null;

    // affichage info level
    public Stars[] stars = new Stars[10];
    private int currentId = 0;
    private int currentWorld = 0;

    public LevelMenu[] levelsMonde1 = new LevelMenu[10];
    public LevelMenu[] levelsMonde2 = new LevelMenu[10];
    public LevelMenu[] levelsMonde3 = new LevelMenu[10];

    public Text nameLeveltext = null;
    public Text nbcouptext = null;
    public Text nbEnnemietext = null;
    public GameObject[] starsSelected = new GameObject[3];

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

        int[] currentList;

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
            default:
                currentList = new int[10] {0,0,0,0,0,0,0,0,0,0};
                break;
        }

        for (int i = 0; i < 10; i++)
        {
            if (currentList[i] <= 0)
            {
                stars[i].starUnlock1.SetActive(false);
                stars[i].starUnlock2.SetActive(false);
                stars[i].starUnlock3.SetActive(false);
            }
            else if (currentList[i] == 1)
            {
                stars[i].starUnlock1.SetActive(true);
                stars[i].starUnlock2.SetActive(false);
                stars[i].starUnlock3.SetActive(false);
            }
            else if(currentList[i] == 2)
            {
                stars[i].starUnlock1.SetActive(true);
                stars[i].starUnlock2.SetActive(true);
                stars[i].starUnlock3.SetActive(false);
            }
            else
            {
                stars[i].starUnlock1.SetActive(true);
                stars[i].starUnlock2.SetActive(true);
                stars[i].starUnlock3.SetActive(true);
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
        currentId = i;

        nameLeveltext.text = "Niveau : " + currentWorld.ToString() + "-" + currentId.ToString();

        int[] currentList0;

        switch (currentWorld)
        {
            case 1:
                currentList0 = starsMonde1;
                break;
            case 2:
                currentList0 = starsMonde2;
                break;
            case 3:
                currentList0 = starsMonde3;
                break;
            default:
                currentList0 = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                break;
        }

        if (currentList0[i] <= 0)
        {
            starsSelected[0].SetActive(false);
            starsSelected[1].SetActive(false);
            starsSelected[2].SetActive(false);
        }
        else if (currentList0[i] == 1)
        {
            starsSelected[0].SetActive(true);
            starsSelected[1].SetActive(false);
            starsSelected[2].SetActive(false);
        }
        else if (currentList0[i] == 2)
        {
            starsSelected[0].SetActive(true);
            starsSelected[1].SetActive(true);
            starsSelected[2].SetActive(false);
        }
        else
        {
            starsSelected[0].SetActive(true);
            starsSelected[1].SetActive(true);
            starsSelected[2].SetActive(true);
        }

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
