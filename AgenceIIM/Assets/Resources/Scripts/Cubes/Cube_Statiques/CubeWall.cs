using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWall : CubeStatic
{
    public override void OnAwake()
    {
        base.OnAwake();

        cubeType = CubeType.Wall;
    }
}