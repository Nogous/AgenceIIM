using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Rewired.Player replayer;
    public bool useWASDLayout;
    public Player player = null;
    private List<Cube> cubes = new List<Cube>();
    public float fallDuration = 1f;
    public float fallSpeed = 1f;
    public int nbEnnemyInit = 1;
    private int nbEnnemy = 1;


    [Header("no Cube")]
    public Texture2D texture2DNoCube;
    public Material matNoCube = null;
    [Header("Base Cube")]
    public Texture2D texture2DCubeBase;
    public Material matCubeBase = null;
    [Header("Wall")]
    public Texture2D texture2DWall;
    public Material matWall = null;
    [Header("Color Grond 1")]
    public Texture2D texture2DGrond1;
    public Material matColor1 = null;
    [Header("Color Grond 2")]
    public Texture2D texture2DGrond2;
    public Material matColor2 = null;
    [Header("Color Grond 3")]
    public Texture2D texture2DGrond3;
    public Material matColor3 = null;
    [Header("EnemyStatic")]
    public Texture2D texture2DEnemyStatic;
    public Material matEnemy = null;
    [Header("Enemy Moving")]
    public Texture2D texture2DEnemyMove;
    public Material matEnemyMove = null;
    [Header("CleaningBox")]
    public Texture2D texture2DCleaningBox;
    public Material matCleaningBox = null;
    [Header("DashBox")]
    public Texture2D texture2DDashBox;
    public Material matDashBox = null;
    [Header("TNT")]
    public Texture2D texture2DTNT;
    public Material matTNT = null;
    [Header("Trigger")]
    public Texture2D texture2DTrigger;
    public Material matTrigger = null;


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
    }

    void Update()
    {
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

    public IEnumerator YouWin()
    {
        yield return new WaitForSeconds(2f);

        ResetParty();
    }

    public void ResetParty()
    {
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
}
