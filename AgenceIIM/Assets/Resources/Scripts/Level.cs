using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform cubePrefab = null;
    public Vector2 levelSize = new Vector2(15,15);

    public Transform[,] cubes = null;
    public Transform[,] enemys = null;
    public int[,] cubesState = null;
    public int[,] enemyState = null;

    public int tabTarget = 0;

    // test
    public Texture2D textureNull = null;
    public Texture2D textureBase = null;
    public Texture2D textureColor1 = null;
    public Texture2D textureColor2 = null;
    public Texture2D textureColor3 = null;

    public Material colorMat0;
    public Material colorMat1;
    public Material colorMat2;
    public Material colorMat3;

    // Start is called before the first frame update
    void Start()
    {
        /*
        //GenerateLevel();
        if (cubesState == null)
        {
            Debug.Log("cubesState est null");
        }
        if (cubes == null)
        {
            Debug.Log("cubes est null");
        }
        */
    }

    public void GenerateLevel()
    {
        string holderCubeName = "Generated Level 0";
        string holderEnemyName = "Generated Level 1";
        if (transform.Find(holderCubeName))
        {
            DestroyImmediate(transform.Find(holderCubeName).gameObject);
            DestroyImmediate(transform.Find(holderEnemyName).gameObject);
        }

        cubes = new Transform[(int)levelSize.x, (int)levelSize.y];
        cubesState = new int[(int)levelSize.x, (int)levelSize.y];

        enemys = new Transform[(int)levelSize.x, (int)levelSize.y];
        enemyState = new int[(int)levelSize.x, (int)levelSize.y];

        // ground
        Transform levelHolder = new GameObject(holderCubeName).transform;
        levelHolder.parent = transform;

        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                Vector3 cubePos = new Vector3(-levelSize.x / 2 + 0.5f + x, 0, -levelSize.y / 2 + 0.5f + y) + transform.position;
                Transform newCube = Instantiate(cubePrefab, cubePos, Quaternion.Euler(Vector3.right * 90)) as Transform;

                cubes[x, y] = newCube;
                cubesState[x, y] = 0;

                newCube.localScale = Vector3.one;
                newCube.parent = levelHolder;
                newCube.gameObject.SetActive(false);
            }
        }

        // level 1
        levelHolder = new GameObject(holderEnemyName).transform;
        levelHolder.parent = transform;

        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                Vector3 cubePos = new Vector3(-levelSize.x / 2 + 0.5f + x, 1, -levelSize.y / 2 + 0.5f + y) + transform.position;
                Transform newCube = Instantiate(cubePrefab, cubePos, Quaternion.Euler(Vector3.right * 90)) as Transform;

                enemys[x, y] = newCube;
                enemyState[x, y] = 0;

                newCube.localScale = Vector3.one;
                newCube.parent = levelHolder;
                newCube.gameObject.SetActive(false);
            }
        }
    }

    public void SetCubesState(int[,] _cubesState)
    {
        cubesState = _cubesState;
    }
}
