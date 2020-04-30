using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int idLevel;

    public static GameManager instance = null;
    public Rewired.Player replayer;
    public bool useWASDLayout;
    public Player player = null;
    private List<Cube> cubes = new List<Cube>();
    public float fallDuration = 1f;
    public float fallSpeed = 1f;
    public int nbEnnemyInit = 1;
    [SerializeField]private int nbEnnemy = 1;

    public string sceneNameToLoad;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DeterminPlatform();
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

        if (replayer.GetButtonDown("Reset"))
        {
            ResetParty();
        }
        if (replayer.GetButtonDown("Pause"))
        {
            Application.Quit();
        }
    }

    public void KillEnnemy()
    {
        nbEnnemy--;

        if (nbEnnemy <= 0)
        {
            StartCoroutine(YouWin());
        }
    }

    public void DeterminPlatform()
    {
        if (!(Application.platform == RuntimePlatform.Android))
        {
            //Code Spécifique PC
            /*GameObject.Find("Mobile_Canvas").SetActive(false);
            GetComponent<SwipeDetector>().enabled = false;*/
        }
        else
        {
            //Code Spécifique Mobile 
            GameObject.Find("Controles_PC").SetActive(false);
        }
    }
    public IEnumerator YouWin()
    {
        DATASaveData();
        yield return new WaitForSeconds(2f);

        //ResetParty();
        SceneManager.LoadScene(sceneNameToLoad);
        
    }

    public void ResetParty()
    {
        DATAnbDeath++;

        player.gameObject.SetActive(true);
        player.ResetPlayer();
        ResetCubes();

        nbEnnemy = nbEnnemyInit;
    }

    #region Cube
    public void AddCube(Cube cube)
    {
        cubes.Add(cube);
    }

    private void ResetCubes()
    {
        for (int i = cubes.Count; i-->0;)
        {
            cubes[i].gameObject.SetActive(true);
            cubes[i].ResetCube();
        }
    }
    #endregion

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
}
