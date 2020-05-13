using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTeleporter : CubeStatic
{
    private Cube teleportDestination = null;
    void Awake()
    {
        cubeType = CubeType.Teleporter;
    }

    
}
