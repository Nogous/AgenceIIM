using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlid : CubeStatic
{
    public override void OnAwake()
    {
        base.OnAwake();

        cubeType = CubeType.TNT;
    }
}
