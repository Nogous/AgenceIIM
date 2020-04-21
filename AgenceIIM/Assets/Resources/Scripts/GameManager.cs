using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;


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

        //ResetParty();
        SceneManager.LoadScene(sceneNameToLoad);
        
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
