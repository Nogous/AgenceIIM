using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDash : CubeStatic
{
    public enum dashEnum
    {
        forward,
        backward,
        right,  
        left, 
    };

    [Header("Options du dash")]
    [SerializeField] public dashEnum dashOrientation = dashEnum.forward;

    public override void OnAwake()
    {
        base.OnAwake();

        cubeType = CubeType.Dash;
    }
}
