using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CubeData
{
    public string id;

    public CubeType cubeType;

    [HideInInspector] public float posX;
    [HideInInspector] public float posY;
    [HideInInspector] public float posZ;

}
