using Rewired.Integration.UnityUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public EventSystem eventSystem;
    public RewiredStandaloneInputModule managerAzerty = null;
    public RewiredStandaloneInputModule managerQwerty = null;

    [Header("Data Points")]
    public StarPoints[] StarPointsMonde1;
    public StarPoints[] StarPointsMonde2;
    public StarPoints[] StarPointsMonde3;

    private int[] starsMonde1;
    private int[] starsMonde2;
    private int[] starsMonde3;

    private int[] unlockWorld;
    public GameObject[] unlockWorldObj;
    public GameObject[] unlockWorldObj2;

    [Header("Pages")]
    public GameObject mainMenuCanvas = null;
    public GameObject worldCanvas = null;
    public GameObject levelCanvas = null;
    public GameObject optionCanvas = null;

    [Header("Stars world")]
    public Text nbStarToUnlockText1;
    public int minPointsToUnlockWorld2 = 10;
    public Button world2Button;
    public Text nbStarToUnlockText2;
    private bool level2Unlock = false;
    public int minPointsToUnlockWorld3 = 20;
    public Button world3Button;
    public Text nbStarToUnlockText3;
    private bool level3Unlock = false;

    public Text nbStarTotal;

    [Header("Level Info")]
    public LevelUI[] levelUIMonde1;
    private int currentId = 0;
    private int currentWorld = 0;

    public LevelMenu[] levelObjMonde1;
    public LevelMenu[] levelObjMonde2;
    public LevelMenu[] levelObjMonde3;

    public Text nameLeveltextTrad = null;
    public Text nameLeveltextEnd = null;
    public Text nbcouptextTrad = null;
    public Text nbcouptextEnd = null;
    public Text nbEnnemietextTrad = null;
    public Text nbEnnemietextEnd = null;
    public GameObject[] starsSelected = new GameObject[3];

    private void Awake()
    {
        loadSaves();
    }

    public void loadSaves()
    {
        unlockWorld = SaveSystem.LoadWorld();

        starsMonde1 = SaveSystem.LoadPoints("starsMonde1");
        starsMonde2 = SaveSystem.LoadPoints("starsMonde2");
        starsMonde3 = SaveSystem.LoadPoints("starsMonde3");

        if (starsMonde1.Length != levelUIMonde1.Length)
        {
            starsMonde1 = new int[levelUIMonde1.Length];
            for (int i = 0; i < starsMonde1.Length; i++)
            {
                starsMonde1[i] = -1;
            }
            SaveSystem.SavePoints(starsMonde1, "starsMonde1");
        }
        if (starsMonde2.Length != levelUIMonde1.Length)
        {
            starsMonde2 = new int[levelUIMonde1.Length];
            for (int i = 0; i < starsMonde2.Length; i++)
            {
                starsMonde2[i] = -1;
            }
            SaveSystem.SavePoints(starsMonde1, "starsMonde2");
        }
        if (starsMonde3.Length != levelUIMonde1.Length)
        {
            starsMonde3 = new int[levelUIMonde1.Length];
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

    public void ResetScore()
    {
        SaveSystem.SavePoints(new int[0], "starsMonde1");
        SaveSystem.SavePoints(new int[0], "starsMonde2");
        SaveSystem.SavePoints(new int[0], "starsMonde3");

        SaveSystem.SaveWorld(new int[2] { 0, 0 });
        loadSaves();
    }

    public void ChangeKeyBind(Text _myText = null)
    {
        GameManager.isQwerty = !GameManager.isQwerty;

        managerAzerty.enabled = !GameManager.isQwerty;
        managerQwerty.enabled = GameManager.isQwerty;
        Debug.Log(GameManager.isQwerty);

        if (_myText != null)
        {
            if (GameManager.isQwerty)
            {
                _myText.text = "Qwerty";
            }
            else
            {
                _myText.text = "Azerty";
            }
        }
    }

    private void Start()
    {
        if (managerAzerty != null && managerQwerty != null)
        {
            managerAzerty.enabled = !GameManager.isQwerty;
            managerQwerty.enabled = GameManager.isQwerty;
        }

        OnClickMainMenu();
    }

    public GameObject MainMenuFirstSelectedObj;
    public GameObject mondeFirstSelectedObj;
    public GameObject levelFirstSelectedObj;
    public GameObject OptionFirstSelectedObj;

    public void OnClickMainMenu()
    {

        mainMenuCanvas.SetActive(true);
        worldCanvas.SetActive(false);
        levelCanvas.SetActive(false);
        optionCanvas.SetActive(false);

        eventSystem.SetSelectedGameObject(MainMenuFirstSelectedObj);
    }

    public void UnlockWorld(int id)
    {
        unlockWorld[id] = 1;
        OnClickSelectWorldMenu();
        SaveSystem.SaveWorld(unlockWorld);
    }

    public void ResetLockWorld()
    {
        SaveSystem.SaveWorld(new int[2] { 0, 0 });
    }

    public void OnClickSelectWorldMenu()
    {
        if (!level2Unlock || !level3Unlock)
        {
            int[] tmpTab;
            StarPoints[] tmpStarStab = new StarPoints[0];
            int[] conte = new int[3];
            int tmpCount = 0;
            for (int i = 1; i <= 3; i++)
            {
                tmpTab = SaveSystem.LoadPoints("starsMonde" + i);
                switch (i)
                {
                    case 1:
                        tmpStarStab = StarPointsMonde1;
                        break;
                    case 2:
                        tmpStarStab = StarPointsMonde2;
                        break;
                    case 3:
                        tmpStarStab = StarPointsMonde3;
                        break;
                }
                for (int j = 0; j < tmpTab.Length; j++)
                {
                    if (tmpTab[j] > 0)
                    {
                        tmpCount += 1;

                        if (tmpTab[j] <= tmpStarStab[j].minPoints2Star)
                        {
                            tmpCount += 1;
                            if (tmpTab[j] <= tmpStarStab[j].minPoints3Star)
                            {
                                tmpCount += 1;
                            }
                        }
                    }
                }
                conte[i - 1] = tmpCount;
                tmpCount = 0;
            }

            tmpCount = conte[0] + conte[1] + conte[2];

            nbStarToUnlockText1.text = conte[0].ToString();

            if (tmpCount >= minPointsToUnlockWorld2)
            {
                if (unlockWorld[0] == 0)
                {
                    unlockWorldObj[0].SetActive(true);
                    unlockWorldObj2[0].SetActive(false);
                }
                else
                {
                    unlockWorldObj[0].SetActive(false);
                    unlockWorldObj2[0].SetActive(false);
                    level2Unlock = true;
                    nbStarToUnlockText2.text = conte[1].ToString();
                }
            }
            else
            {
                unlockWorldObj[0].SetActive(false);
                unlockWorldObj2[0].SetActive(true);
                nbStarToUnlockText2.text = tmpCount+ " / " + minPointsToUnlockWorld2;
            }
            if (tmpCount >= minPointsToUnlockWorld3)
            {
                if (unlockWorld[1] == 0)
                {
                    unlockWorldObj[1].SetActive(true);
                    unlockWorldObj2[1].SetActive(false);
                }
                else
                {
                    unlockWorldObj[1].SetActive(false);
                    unlockWorldObj2[1].SetActive(false);

                    level3Unlock = true;
                    nbStarToUnlockText3.text = conte[2].ToString();
                }
            }
            else
            {
                unlockWorldObj[1].SetActive(false);
                unlockWorldObj2[1].SetActive(true);
                nbStarToUnlockText3.text = tmpCount + " / " + minPointsToUnlockWorld3;
            }

            nbStarTotal.text = tmpCount + " / " + 135;
        }

        if (world2Button != null)
        {
            world2Button.interactable = level2Unlock;
        }
        else
        {
            Debug.Log("world2Button is not set");
        }

        if (world3Button != null)
        {
            world3Button.interactable = level3Unlock;
        }
        else
        {
            Debug.Log("world2Button is not set");
        }

        mainMenuCanvas.SetActive(false);
        worldCanvas.SetActive(true);
        levelCanvas.SetActive(false);
        optionCanvas.SetActive(false);

        eventSystem.SetSelectedGameObject(mondeFirstSelectedObj);
    }

    public void OnClickOption()
    {
        optionCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        levelCanvas.SetActive(false);
        worldCanvas.SetActive(false);


        eventSystem.SetSelectedGameObject(OptionFirstSelectedObj);
    }

    public void OnClickSelectLevelMenu(int id)
    {
        currentWorld = id;
        mainMenuCanvas.SetActive(false);
        levelCanvas.SetActive(true);
        worldCanvas.SetActive(false);
        optionCanvas.SetActive(false);

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
            if (tmpPoints[i].sprite != null)
            {
                levelUIMonde1[i].image.sprite = tmpPoints[i].sprite;
            }

            if (levelUIMonde1[i].button != null)
            {
                levelUIMonde1[i].button.interactable = false;
            }
            if (levelUIMonde1[i].locker != null)
            {
                levelUIMonde1[i].locker.SetActive(true);
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
                if (levelUIMonde1[i].locker != null)
                {
                    levelUIMonde1[i].locker.SetActive(false);
                }
                if (levelUIMonde1.Length > i+1)
                {
                    if (levelUIMonde1[i+1].button != null)
                    {
                        levelUIMonde1[i+1].button.interactable = true;
                    }
                    if (levelUIMonde1[i+1].locker != null)
                    {
                        levelUIMonde1[i+1].locker.SetActive(false);
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
        OnclikSelecteLevel(1);
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
        
        nameLeveltextEnd.text = string.Format(nameLeveltextTrad.text, currentWorld.ToString(), (currentId+1).ToString());
        


        LevelMenu[] currentList = levelObjMonde1;
        StarPoints[] currenPoint = StarPointsMonde1;
        switch (currentWorld)
        {
            case 2:
                currentList = levelObjMonde2;
                currenPoint = StarPointsMonde2;
                break;
            case 3:
                currentList = levelObjMonde3;
                currenPoint = StarPointsMonde3;
                break;
        }

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
                // info level
                nbcouptextEnd.text = string.Format(nbcouptextTrad.text, currenPoint[currentId].nbblock);
                nbEnnemietextEnd.text = string.Format(nbEnnemietextTrad.text, currenPoint[currentId].nbenemie);

                // spawn level
                currentLevel.gameObject.SetActive(true);
                currentLevel.LoadLevel();
            }
        }
    }



    public void LoadLastLevel()
    {
        string toLoadId = "3-14";

        for (int j = 4; j-->0;)
        {
            int[] currentList = starsMonde1;
            switch (j)
            {
                case 3:
                    currentList = starsMonde3;
                    break;
                case 2:
                    currentList = starsMonde2;
                    break;
                case 1:
                    currentList = starsMonde1;
                    break;
                case 0:
                    SceneManager.LoadScene("1-0");
                    return;
                default:
                    Debug.Log("WAT?");
                    break;
            }

            for (int i = currentList.Length; i-- > 0;)
            {
                if (currentList[i] < 1)
                {
                    toLoadId = j.ToString() + "-" + i.ToString();
                }
                else
                {
                    SceneManager.LoadScene(toLoadId);
                    return;
                }
            }
        }
    }

    #region cheatcodes


    #endregion

    #region Audio

    void PlayButtonPressed()
    {
        
    }

    #endregion
}

[System.Serializable]
public class StarPoints
{
    public int minPoints3Star;
    public int minPoints2Star;
    public Sprite sprite;
    public int nbenemie;
    public int nbblock;
}


