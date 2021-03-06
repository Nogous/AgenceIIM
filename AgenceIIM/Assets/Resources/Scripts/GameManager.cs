﻿using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

[System.Serializable]
public enum Monde
{
    Monde1,
    Monde2,
    Monde3,
}

public class GameManager : MonoBehaviour
{
    public Monde idMonde;
    public int idLevel;

    public static GameManager instance = null;
    public static bool isQwerty = false;

    public CameraTravel cameraTravel;

    public Rewired.Player replayer;
    public bool useWASDLayout;
    public Player player = null;
    [HideInInspector] public List<Enemy> cubesEnnemy = new List<Enemy>();
    public float fallDuration = 1f;
    public float fallSpeed = 1f;
    public int nbEnnemyInit = 1;
    [SerializeField]private int nbEnnemy = 1;

    public string sceneNameToLoad;

    public GameObject UI_mobile;

    public int minPoints3Star = 10;
    public int minPoints2Star = 10;

    // color Rainbow
    public Material rainbowMaterial = null;
    public float colorSpeed = 1f;
    private float rainbowColor = 0f;

    public event Action OnResetLevel;
    public UIEndLevel uiEndLevel;

    public Text txtNbCoups;

    public float timeEndLevelWin = 1f;

    private void Awake()
    {
        DeterminPlatform();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        uiEndLevel.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        replayer = ReInput.players.GetPlayer(0);
        player.replayer = replayer;
        if (!useWASDLayout)
        {
            replayer.controllers.maps.SetMapsEnabled(true, 0);
        }
        else
        {
            replayer.controllers.maps.SetMapsEnabled(true, 2);
        }
        nbEnnemy = nbEnnemyInit;

        if (SpawnLevel.Instance != null)
        {
            SpawnLevel.Instance.StartSpawnLevel();
        }
    }

    void Update()
    {
        DATATimeInTheGame();

        if (replayer.GetButtonDown("Reset") && player.canReset)
        {
            if (!isWining && !player.isDeaing)
            {
                ResetParty();
            }
        }
        if (replayer.GetButtonDown("Pause"))
        {
            //Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToMainMenu();
        }

        UpdateColorRainbow();
    }

    public void GoToMainMenu()
    {
        player.resetOnMove();
        SceneManager.LoadScene(0);
        Destroy(GameObject.Find("Musique(Clone)"));
    }

    public void UpdateColorRainbow()
    {
        if (rainbowMaterial == null) return;

        rainbowColor = Time.time * colorSpeed %1;
        rainbowMaterial.color = Color.HSVToRGB(rainbowColor,1,1);
    }

    public void KillEnnemy()
    {
        nbEnnemy--;

        if (nbEnnemy <= 0)
        {
            StartCoroutine(YouWin());
        }
    }

    public void TNTExplode()
    {
        if(player.DoAction == player.DoActionWait)player.TestTile(true);

        for (int i = cubesEnnemy.Count; i-->0;)
        {
            cubesEnnemy[i].TestTile();
        }
    }

    public void DeterminPlatform()
    {
        GameObject.Find("UI_PC").SetActive(true);
        GameObject.Find("UI_Mobile").SetActive(true);
#if UNITY_EDITOR
        if (!(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android))
        {
            //Code Spécifique PC
            Destroy(GameObject.Find("UI_Mobile"));
        }
        else
        {
            // Code Spécifique Mobile
            Destroy(GameObject.Find("UI_PC"));
        }
#else
        if (!(Application.platform == RuntimePlatform.Android))
        {
            //Code Spécifique PC
            Destroy(GameObject.Find("UI_Mobile"));
        }
        else
        {
            //Code Spécifique Mobile 
            Destroy(GameObject.Find("UI_PC"));
        }
#endif
    }

    private bool isWining = false;

    public IEnumerator YouWin()
    {
        isWining = true;

        yield return new WaitForSeconds(.2f);
        SpawnLevel.Instance.StartUnPopLevel();
        DATASaveData();
        yield return new WaitForSeconds(timeEndLevelWin);

        // save points
        switch (idMonde)
        {
            case Monde.Monde1:
                SaveSystem.SetPoints("starsMonde1", idLevel, player.nbMove);
                break;
            case Monde.Monde2:
                SaveSystem.SetPoints("starsMonde2", idLevel, player.nbMove);
                break;
            case Monde.Monde3:
                SaveSystem.SetPoints("starsMonde3", idLevel, player.nbMove);
                break;
            default:
                break;
        }

        uiEndLevel.gameObject.SetActive(true);
        // info ici
        uiEndLevel.textStar1.text = "niveau fini";

        uiEndLevel.textStar2End.text = string.Format(uiEndLevel.textStar2.text, minPoints2Star.ToString());
        uiEndLevel.textStar3End.text = string.Format(uiEndLevel.textStar3.text, minPoints3Star.ToString());

        uiEndLevel.LoadStars(player.nbMove, minPoints2Star, minPoints3Star);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void ResetParty(float time = 0)
    {
        DATAnbDeath++;

        StartCoroutine(DoReset(time));
    }

    private IEnumerator DoReset(float time = 0)
    {
        yield return new WaitForSeconds(time);

        player.ResetPlayerMove();
        OnResetLevel();
        nbEnnemy = nbEnnemyInit;
        player.MobileAxeHorNeg = false;
        player.MobileAxeHorPos = false;
        player.MobileAxeVerNeg = false;
        player.MobileAxeVerPos = false;
        player.gameObject.SetActive(true);
    }

    #region analitics
    public void DATASaveData()
    {
        if (PlaytestAnalitic.Instance != null)
        {
            
            PlaytestAnalitic.Instance.timeDuration[idLevel] = DATA_Time;
            PlaytestAnalitic.Instance.nbDeath[idLevel] = DATAnbDeath;
            PlaytestAnalitic.Instance.nbMoveCam[idLevel] = DATAnbMoveCam;

            PlaytestAnalitic.Instance.ShowData(idLevel);
        }
    }

    float DATA_Time = 0;
    int DATAnbDeath = 0;
    public int DATAnbMoveCam = 0;

    public void DATATimeInTheGame()
    {
        DATA_Time += Time.deltaTime;
    }
    #endregion

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
