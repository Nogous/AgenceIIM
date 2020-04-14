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
    }

    void Update()
    {
        if (replayer.GetButtonDown("Reset"))
        {
            ResetParty();
        }
    }

    public void ResetParty()
    {
        player.gameObject.SetActive(true);
        player.ResetPlayer();
        ResetCubes();
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
