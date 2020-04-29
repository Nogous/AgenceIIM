using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    private bool isSpawnConpleat = false;
    private bool isSpawnConpleat2 = false;

    public float hightSpawn = 1f;
    public float fallSpeed = 1f;

    public List<GameObject> cubes = new List<GameObject>();
    private List<Vector3> cubePos = new List<Vector3>();

    private float fLerp = 0f;
    private int idCube = 0;

    private float fLerp2 = -.5f;
    private int idCube2 = 1;

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.Play("");

        for (int i = 0; i < cubes.Count; i++)
        {
            cubePos.Add(cubes[i].transform.position);
            cubes[i].transform.position = cubePos[idCube] + Vector3.up * hightSpawn;
        }
    }
    private void Update()
    {
        if (!isSpawnConpleat)
        {
            UpdateFallLevel();
        }
        if (!isSpawnConpleat2)
        {
            UpdateFallLevel2();
        }
    }

    public void UpdateFallLevel()
    {
        fLerp += Time.deltaTime * fallSpeed;
        cubes[idCube].transform.position = Vector3.Lerp(cubePos[idCube] + Vector3.up* hightSpawn, cubePos[idCube], fLerp);

        if (fLerp >= 1f)
        {
            idCube +=2;
            fLerp = 0f;
            if (idCube >= cubes.Count)
            {
                isSpawnConpleat = true;
            }
        }
    }

    public void UpdateFallLevel2()
    {
        fLerp2 += Time.deltaTime * fallSpeed;
        cubes[idCube2].transform.position = Vector3.Lerp(cubePos[idCube2] + Vector3.up * hightSpawn, cubePos[idCube2], fLerp2);

        if (fLerp2 >= 1f)
        {
            idCube2 += 2;
            fLerp2 = 0f;
            if (idCube2 >= cubes.Count)
            {
                isSpawnConpleat2 = true;
            }
        }
    }
}
