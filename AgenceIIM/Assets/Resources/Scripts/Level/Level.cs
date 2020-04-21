using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CubeType
{
    Base,
    Ennemy
}

[System.Serializable]
public class CubeData
{
    public string id;

    public CubeType cubeType;

    [HideInInspector]
    public Vector3 position;

    [HideInInspector]
    public Quaternion rotation;
}

public class Level : MonoBehaviour
{
    public int i = 1;

}
