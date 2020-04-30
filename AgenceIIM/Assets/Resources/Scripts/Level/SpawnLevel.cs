using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    public static SpawnLevel Instance = null;

    private bool isSpawnConpleat = true;
    private bool isUnPopConpleat = true;

    public float hightSpawn = 1f;
    public float fallSpeed = 1f;
    public int nbsimultaneousFallingObject = 1;

    public List<GameObject> cubes = new List<GameObject>();
    private List<Vector3> cubePos = new List<Vector3>();

    private float[] fLerp;
    private int[] idCube;
    private bool[] isFall;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("more than 1 SpawnLevel");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.Play("");

        for (int i = 0; i < cubes.Count; i++)
        {
            cubePos.Add(cubes[i].transform.position);
            cubes[i].transform.position = cubePos[i] + Vector3.up * hightSpawn;
        }

        fLerp = new float[nbsimultaneousFallingObject];
        idCube = new int[nbsimultaneousFallingObject];
        isFall = new bool[nbsimultaneousFallingObject];

        for (int i = 0; i < nbsimultaneousFallingObject; i++)
        {
            fLerp[i] = -((float)i / (float)nbsimultaneousFallingObject);
            idCube[i] = i;
            isFall[i] = true;
        }
    }
    private void Update()
    {
        if (!isSpawnConpleat)
        {
            UpdateFallLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpawnLevel();
        }

        if (!isUnPopConpleat)
        {
            UpdateFloatLevel();
        }
    }

    public void StartSpawnLevel()
    {
        for (int i = 0; i < cubes.Count; i++)
            cubes[i].transform.position = cubePos[i] + Vector3.up * hightSpawn;
        for (int i = 0; i < nbsimultaneousFallingObject; i++)
        {
            fLerp[i] = -((float)i / (float)nbsimultaneousFallingObject);
            idCube[i] = i;
            isFall[i] = true;
        }
        isSpawnConpleat = false;
        isUnPopConpleat = true;
    }

    private void UpdateFallLevel()
    {
        for (int i = nbsimultaneousFallingObject; i-- > 0;)
        {
            if (isFall[i])
            {
                fLerp[i] += Time.deltaTime * fallSpeed;
                cubes[idCube[i]].transform.position = Vector3.Lerp(cubePos[idCube[i]] + Vector3.up * hightSpawn, cubePos[idCube[i]], fLerp[i]);

                if (fLerp[i] >= 1f)
                {
                    idCube[i] += nbsimultaneousFallingObject;
                    fLerp[i] = 0f;
                    if (idCube[i] >= cubes.Count)
                    {
                        isFall[i] = false;
                        if (i == nbsimultaneousFallingObject-1)
                        {
                            EndSpawnLevel();
                        }
                    }
                }
            }
        }
    }

    private void EndSpawnLevel()
    {
        isSpawnConpleat = true;
        for (int i = nbsimultaneousFallingObject; i-->0;)
        {
            isFall[i] = true;
        }
    }

    public void StartUnPopLevel()
    {
        for (int i = 0; i < cubes.Count; i++)
            cubes[i].transform.position = cubePos[i];
        for (int i = 0; i < nbsimultaneousFallingObject; i++)
        {
            fLerp[i] = -((float)i / (float)nbsimultaneousFallingObject);
            idCube[i] = i;
            isFall[i] = true;
        }

        isSpawnConpleat = true;
        isUnPopConpleat = false;
    }

    private void UpdateFloatLevel()
    {
        for (int i = nbsimultaneousFallingObject; i-- > 0;)
        {
            if (isFall[i])
            {
                fLerp[i] += Time.deltaTime * fallSpeed;
                cubes[idCube[i]].transform.position = Vector3.Lerp(cubePos[idCube[i]], cubePos[idCube[i]] + Vector3.up * hightSpawn, fLerp[i]);

                if (fLerp[i] >= 1f)
                {
                    idCube[i] += nbsimultaneousFallingObject;
                    fLerp[i] = 0f;
                    if (idCube[i] >= cubes.Count)
                    {
                        isFall[i] = false;
                        EndUnPopLevel();
                    }
                }
            }
        }
    }

    private void EndUnPopLevel()
    {
        isUnPopConpleat = true;
        for (int i = nbsimultaneousFallingObject; i-- > 0;)
        {
            isFall[i] = true;
        }
    }
}
