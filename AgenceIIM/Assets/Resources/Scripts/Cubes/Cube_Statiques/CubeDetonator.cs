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
    public CubeTNT[] associatedTnt = new CubeTNT[0];

    [ExecuteInEditMode]
    public override void OnAwake()
    {
        base.OnAwake();

        isBreakable = false;

        cubeType = CubeType.Detonator;
    }

    public void ActivateTnt()
    {
        for (int i = 0; i < associatedTnt.Length; i++)
        {
            if (associatedTnt[i] != null && associatedTnt[i].gameObject.activeSelf)
            {
                associatedTnt[i].Explode();
            }
        }
    }

    public void DrawFuse(Color fuseColor)
    {
        Transform tmpTNT;

        for (int i = 0; i < associatedTnt.Length; i++)
        {
            if (associatedTnt[i] == null)
            {
                return;
            }

            tmpTNT = associatedTnt[i].transform;

            if (fuseManhanttanOrder == fuseOrientationMode.HorrizontalFirst)
            {

                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(transform.position.x, transform.position.y + fuseOffset, tmpTNT.position.z), fuseColor);
                if (!(transform.position.y < tmpTNT.position.y))
                {
                    Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, tmpTNT.position.z), new Vector3(tmpTNT.position.x, tmpTNT.position.y - fuseOffset, tmpTNT.position.z), fuseColor);
                }
                else
                {
                    Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, tmpTNT.position.z), new Vector3(tmpTNT.position.x, tmpTNT.position.y + fuseOffset, tmpTNT.position.z), fuseColor);
                }
            }
            else if (fuseManhanttanOrder == fuseOrientationMode.VerticalFirst)
            {
                Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(tmpTNT.position.x, transform.position.y + fuseOffset, transform.position.z), fuseColor);
                if (!(transform.position.y < tmpTNT.position.y))
                {
                    Debug.DrawRay(new Vector3(tmpTNT.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(tmpTNT.position.x, tmpTNT.position.y - fuseOffset, tmpTNT.position.z), fuseColor);
                }
                else
                {
                    Debug.DrawRay(new Vector3(tmpTNT.position.x, transform.position.y + fuseOffset, transform.position.z), new Vector3(tmpTNT.position.x, tmpTNT.position.y + fuseOffset, tmpTNT.position.z), fuseColor);
                }
            }
        }
    }

    void Update()
    {
        DrawFuse(fuseColor);
    }

}
