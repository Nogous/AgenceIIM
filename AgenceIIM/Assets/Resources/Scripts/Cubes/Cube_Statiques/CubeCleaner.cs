using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCleaner : CubeStatic
{
    public override void OnAwake()
    {
        base.OnAwake();

        isBreakable = false;

        cubeType = CubeType.Cleaner;
    }
}
