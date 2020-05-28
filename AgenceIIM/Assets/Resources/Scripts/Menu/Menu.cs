using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class StarPoints
{
    public int minPoints3Star;
    public int minPoints2Star;
}

public class Menu : MonoBehaviour
{
    public EventSystem eventSystem;

    [Header("Data Points")]
    public StarPoints[] StarPointsMonde1;
    public StarPoints[] StarPointsMonde2;
    public StarPoints[] StarPointsMonde3;

    private int[] starsMonde1;
    private int[] starsMonde2;
    private int[] starsMonde3;

    [Header("Pages")]
    public GameObject mainMenuCanvas = null;
    public GameObject worldCanvas = null;
    public GameObject levelCanvas = null;

    [Header("Level Info")]
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

        starsMonde1 = SaveSystem.LoadPoints("starsMonde1");
        starsMonde2 = SaveSystem.LoadPoints("starsMonde2");
        starsMonde3 = SaveSystem.LoadPoints("starsMonde3");

        if (starsMonde1.Length != levelUIMonde1.Length)
        {
            Debug.Log("reset points Mode 1");
            starsMonde1 = new int[levelUIMonde1.Length];
            for (int i = 0; i < starsMonde1.Length; i++)
            {
                starsMonde1[i] = -1;
            }
            SaveSystem.SavePoints(starsMonde1, "starsMonde1");
        }
        if (starsMonde2.Length != levelUIMonde2.Length)
        {
            starsMonde2 = new int[levelUIMonde2.Length];
            for (int i = 0; i < starsMonde2.Length; i++)
            {
                starsMonde2[i] = -1;
            }
            SaveSystem.SavePoints(starsMonde1, "starsMonde2");
        }
        if (starsMonde3.Length != levelUIMonde3.Length)
        {
            starsMonde3 = new int[levelUIMonde3.Length];
            for (int i = 0; i < starsMonde3.Length; i++)
            {
                starsMonde3[i] = -1;
            }
            SaveSystem.SavePoints(starsMonde1, "starsMonde3");
        }

        int index = 1;

        for (int i = 0; i < levelUIMonde1.Length; i++)
        {
            levelUIMonde1[i].idLevelText.text = index.ToString();
            levelUIMonde1[i].idLevel = index;
            index++;
        }

    }

    private void Start()
    {
        OnClickMainMenu();
    }

    public GameObject MainMenuFirstSelectedObj;
    public GameObject mondeFirstSelectedObj;
    public GameObject levelFirstSelectedObj;

    public void OnClickMainMenu()
    {

        mainMenuCanvas.SetActive(true);
        worldCanvas.SetActive(false);
        levelCanvas.SetActive(false);

        eventSystem.SetSelectedGameObject(MainMenuFirstSelectedObj);
    }

    public void OnClickSelectWorldMenu()
    {
        mainMenuCanvas.SetActive(false);
        worldCanvas.SetActive(true);
        levelCanvas.SetActive(false);

        eventSystem.SetSelectedGameObject(mondeFirstSelectedObj);
    }

    public void OnClickSelectLevelMenu(int id)
    {
        currentWorld = id;
        mainMenuCanvas.SetActive(false);
        levelCanvas.SetActive(true);
        worldCanvas.SetActive(false);

        int[] currentList = new int[0];


        StarPoints[] tmpPoints = new StarPoints[0];

        switch (id)
        {
            case 1:
                currentList = starsMonde1;
                tmpPoints = StarPointsMonde1;
                break;
            case 2:
                currentList = starsMonde2;
                tmpPoints = StarPointsMonde2;
                break;
            case 3:
                currentList = starsMonde3;
                tmpPoints = StarPointsMonde3;
                break;
        }

        for (int i = 0; i < currentList.Length; i++)
        {
            if (levelUIMonde1[i].button != null)
            {
                levelUIMonde1[i].button.interactable = false;
            }
        }

        for (int i = 0; i < currentList.Length; i++)
        {
            levelUIMonde1[i].starUnlock1.SetActive(false);
            levelUIMonde1[i].starUnlock2.SetActive(false);
            levelUIMonde1[i].starUnlock3.SetActive(false);

            if (currentList[i] >= 0)
            {
                if (levelUIMonde1[i].button != null)
                {
                    levelUIMonde1[i].button.interactable = true;
                }
                if (levelUIMonde1.Length > i)
                {
                    if (levelUIMonde1[i].button != null)
                    {
                        levelUIMonde1[i].button.interactable = true;
                    }
                }

                levelUIMonde1[i].starUnlock1.SetActive(true);


                if (currentList[i] <= tmpPoints[i].minPoints2Star)
                {
                    levelUIMonde1[i].starUnlock2.SetActive(true);
                    if (currentList[i] <= tmpPoints[i].minPoints3Star)
                    {
                        levelUIMonde1[i].starUnlock3.SetActive(true);
                    }
                }
            }
        }

        eventSystem.SetSelectedGameObject(levelFirstSelectedObj);
    }

    public void OnClickStartLevel()
    {
        SceneManager.LoadScene(currentWorld.ToString() + "-" + currentId.ToString());
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

    #region cheatcodes


    public void OnClickOption()
    {
        if (CheatCodes.Instance != null)
        {

        }
    }

    #endregion
}
