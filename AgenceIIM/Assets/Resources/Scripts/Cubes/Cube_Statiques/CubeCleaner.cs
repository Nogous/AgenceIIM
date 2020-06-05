using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCleaner : CubeStatic
{
    public override void OnAwake()
    {
        base.OnAwake();

        isBreakable = false;

        cubeType = CubeType.Cleaner;
    }

    public void Clean(Player playertoclean)
    {
        if(playertoclean.faceColor[1].GetComponent<Renderer>().material.color != playertoclean.baseColor)
        {
            playertoclean.faceColor[1].GetComponent<Renderer>().material.color = playertoclean.baseColor;
            AudioManager.instance.Play("cleanSFX");
        }
    }
}
