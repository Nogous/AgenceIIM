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

    [SerializeField] private GameObject arrow = null;

    public override void OnAwake()
    {
        base.OnAwake();

        isBreakable = false;

        cubeType = CubeType.Dash;

        if(dashOrientation == dashEnum.forward)
        {
            arrow.transform.eulerAngles = new Vector3(90, 270, 0);
        }
        else if (dashOrientation == dashEnum.backward)
        {
            arrow.transform.eulerAngles = new Vector3(90, 90, 0);
        }
        else if (dashOrientation == dashEnum.right)
        {
            arrow.transform.eulerAngles = new Vector3(90, 0, 0);
        }
        else 
        {
            arrow.transform.eulerAngles = new Vector3(90, 180, 0);

        }
    }
}
