using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWall : CubeStatic
{
    public override void Awake()
    {
        cubeType = CubeType.Wall;
    }
}