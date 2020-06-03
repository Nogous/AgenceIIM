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

    [ExecuteInEditMode]
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
        if(associatedTnt == null){
            return;
        }
        if(fuseManhanttanOrder == fuseOrientationMode.HorrizontalFirst)
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(transform.position.x, transform.position.y + fuseOffset, associatedTnt.transform.position.z), fuseColor);
            if(!(transform.position.y < associatedTnt.transform.position.y)){
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, associatedTnt.transform.position.z), new Vector3(associatedTnt.transform.position.x, associatedTnt.transform.position.y - fuseOffset, associatedTnt.transform.position.z), fuseColor);
            }
            else
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, associatedTnt.transform.position.z), new Vector3(associatedTnt.transform.position.x, associatedTnt.transform.position.y + fuseOffset, associatedTnt.transform.position.z), fuseColor);
            } 
        }
        else if(fuseManhanttanOrder == fuseOrientationMode.VerticalFirst)
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(associatedTnt.transform.position.x, transform.position.y + fuseOffset, transform.position.z), fuseColor);
            if(!(transform.position.y < associatedTnt.transform.position.y)){
                Debug.DrawRay(new Vector3(associatedTnt.transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(associatedTnt.transform.position.x, associatedTnt.transform.position.y - fuseOffset, associatedTnt.transform.position.z), fuseColor);
            }
            else
            {
                Debug.DrawRay(new Vector3(associatedTnt.transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(associatedTnt.transform.position.x, associatedTnt.transform.position.y + fuseOffset, associatedTnt.transform.position.z), fuseColor);
            }
        }
    }

    void Update()
    {
        DrawFuse(fuseColor);
    }

}
