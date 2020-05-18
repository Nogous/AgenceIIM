using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDetonator : CubeStatic
{
    public CubeTNT associatedTnt;
    public override void Awake()
    {
        cubeType = CubeType.Detonator;
    }

    public void ActivateTnt()
    {
        if (associatedTnt != null && associatedTnt.gameObject.activeSelf)
        {
            associatedTnt.Explode();
        }
    }
}
