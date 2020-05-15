using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWall : CubeStatic
{
    void Awake()
    {
        cubeType = CubeType.Wall;
    }
}