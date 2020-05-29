using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDetonator : CubeStatic
{
    public enum fuseOrientationMode{
        HorrizontalFirst,
        VerticalFirst,
    }
    public float fuseOffset;
    public bool fuseOrientation;
    public fuseOrientationMode fuseManhanttanOrder;
    public Color fuseColor;
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

    public void DrawFuse(Color fuseColor)
    {
        if(fuseManhanttanOrder == fuseOrientationMode.HorrizontalFirst){
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, transform.position.z), transform.TransformVector(Vector3.forward) * (System.Math.Abs(associatedTnt.transform.position.x - transform.position.x)), fuseColor);
            Debug.DrawRay(new Vector3(System.Math.Abs(associatedTnt.transform.position.x - transform.position.x), transform.position.y + fuseOffset, transform.position.z), associatedTnt.transform.position, fuseColor);
        }
        
    }

    [ExecuteInEditMode]
    void Update()
    {
        DrawFuse(fuseColor);
    }

}
