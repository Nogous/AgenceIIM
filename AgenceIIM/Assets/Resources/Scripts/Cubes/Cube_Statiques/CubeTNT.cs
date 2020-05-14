using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTNT : CubeStatic
{
    [SerializeField] private float tntDelay = 1f;
    [SerializeField] private ParticleSystem particleTnt = null;
    private Cube associatedTnt = null;
    void Awake()
    {
        cubeType = CubeType.TNT;
    }

}
