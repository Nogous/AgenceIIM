using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] private KeyCode resetGameKey = KeyCode.R;

    public Player player;
    private List<Cube> cubes = new List<Cube>();

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

    void Update()
    {
        if (Input.GetKeyDown(resetGameKey))
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
