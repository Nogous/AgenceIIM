using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestructible : CubeStatic
{

    public override void Awake()
    {
        cubeType = CubeType.Destructible;
    }
    
}
