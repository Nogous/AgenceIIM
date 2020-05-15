using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTeleporter : CubeStatic
{
    [SerializeField] private Cube teleportDestination = null;
    void Awake()
    {
        cubeType = CubeType.Teleporter;
    }

    public Cube GetDestination()
    {
        return teleportDestination;
    }
}
