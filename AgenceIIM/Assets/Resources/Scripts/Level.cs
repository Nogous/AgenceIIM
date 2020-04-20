using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform cubePrefab = null;
    public Vector2 levelSize = new Vector2(15,15);

    public Transform[] cubes = new Transform[225];
    public Transform[] enemys = new Transform[225];
    public int[] cubesState = new int[225];
    public int[] enemyState = new int[225];

    public int tabTarget = 0;


    [Header("no Cube")]
    public Texture2D texture2DNoCube;
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

        cubes = new Transform[(int)levelSize.x* (int)levelSize.y];
        cubesState = new int[(int)levelSize.x* (int)levelSize.y];

        enemys = new Transform[(int)levelSize.x* (int)levelSize.y];
        enemyState = new int[(int)levelSize.x* (int)levelSize.y];

        // ground
        Transform levelHolder = new GameObject(holderCubeName).transform;
        levelHolder.parent = transform;

        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                Vector3 cubePos = new Vector3(-levelSize.x / 2 + 0.5f + x, 0, -levelSize.y / 2 + 0.5f + y) + transform.position;
                Transform newCube = Instantiate(cubePrefab, cubePos, Quaternion.Euler(Vector3.right * 90)) as Transform;

                cubes[x+ (y* (int)levelSize.x)] = newCube;
                cubesState[x + (y * (int)levelSize.x)] = 0;

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

                enemys[x + (y * (int)levelSize.x)] = newCube;
                enemyState[x + (y * (int)levelSize.x)] = 0;

                newCube.localScale = Vector3.one;
                newCube.parent = levelHolder;
                newCube.gameObject.SetActive(false);
            }
        }
    }
}
