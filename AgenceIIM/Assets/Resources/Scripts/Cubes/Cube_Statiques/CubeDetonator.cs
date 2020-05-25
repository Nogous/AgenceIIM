using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDetonator : CubeStatic
{
    public CubeTNT associatedTnt;

    public override void OnAwake()
    {
        base.OnAwake();

        isBreakable = false;

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
