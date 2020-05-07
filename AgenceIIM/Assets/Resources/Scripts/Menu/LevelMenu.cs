using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public List<GameObject> cubes;
    public List<GameObject> cubesTmp = new List<GameObject>();
    public int nbFallObjSimultanated = 1;
    public float fallSpeed = 1;
    private MeshRenderer[] meshes;
    private Vector3[] initPos;
    private int currentCube = 0;

    public float spawnHeight = 20f;

    public void Awake()
    {
        meshes = new MeshRenderer[cubes.Count];
        initPos = new Vector3[cubes.Count];
        
        for (int i = 0; i < cubes.Count; i++)
        {
            cubesTmp.Add(cubes[i]);
        }

        for (int i = 0; i < cubes.Count; i++)
        {
            meshes[i] = cubes[i].GetComponent<MeshRenderer>();
            meshes[i].enabled = false;

            initPos[i] = cubes[i].transform.position;

            cubes[i].transform.position += Vector3.up * spawnHeight;
        }
    }

    public void InitLoad()
    {
        StopAllCoroutines();
        currentCube = 0;
        cubesTmp = new List<GameObject>();
        for (int i = 0; i < cubes.Count; i++)
        {
            cubesTmp.Add(cubes[i]);
        }

        for (int i = 0; i < cubes.Count; i++)
        {
            falls = new List<Fall>();
            cubes[i].transform.position = initPos[i] + Vector3.up * spawnHeight;
        }
    }

    public void LoadLevel()
    {
        InitLoad();
        for (int i = cubes.Count; i-- > 0;)
        {
            meshes[i].enabled = true;
        }
        for (int i = 0; i < nbFallObjSimultanated;i++)
        {
            StartCoroutine(StartFall((float)i / (float)nbFallObjSimultanated));
        }
    }

    public IEnumerator StartFall(float time)
    {
        if (time != 0)
        {
            yield return new WaitForSeconds(time);
        }
        EndFall();
    }

    public void EndFall()
    {
        if (cubesTmp.Count > 0)
        {
            StartFallCube(cubesTmp[0], cubesTmp[0].transform.position, initPos[currentCube], EndFall, fallSpeed);
            currentCube++;
            cubesTmp.RemoveAt(0);
        }
        else if (falls.Count <= 1)
        {
            Debug.Log("endSpawn");
        }
    }

    void Update()
    {
        FallCube();
    }

    private List<Fall> falls = new List<Fall>();

    public void StartFallCube(GameObject cube, Vector3 initPos, Vector3 endPos, Action callBack = null, float speed = 1)
    {
        falls.Add(new Fall(cube, initPos, endPos, callBack, speed));
    }

    public void FallCube()
    {
        if (falls.Count == 0) return;

        for (int i = falls.Count; i -->0;)
        {
            falls[i].lerp += Time.deltaTime * falls[i].fallSpeed;
            falls[i].obj.transform.position = Vector3.Lerp(falls[i].initPos, falls[i].endPos, falls[i].lerp);
            if (falls[i].lerp > 1)
            {
                if (falls[i].callBack != null)
                {
                    falls[i].callBack();
                }
                falls.Remove(falls[i]);
            }
        }
    }
}

public class Fall
{
    public GameObject obj;
    public Vector3 initPos;
    public Vector3 endPos;
    public Action callBack;
    public float fallSpeed;
    public float lerp = 0;

    public Fall(GameObject _obj, Vector3 _initPos, Vector3 _endPos, Action _callBack = null, float _fallSpeed = 1f)
    {
        obj = _obj;
        initPos = _initPos;
        endPos = _endPos;
        callBack = _callBack;
        fallSpeed = _fallSpeed;
    }

    public Fall(GameObject _obj, Vector3 _initPos, Vector3 _endPos, float _fallSpeed = 1f)
    {
        obj = _obj;
        initPos = _initPos;
        endPos = _endPos;
        callBack = null;
        fallSpeed = _fallSpeed;
    }
}
