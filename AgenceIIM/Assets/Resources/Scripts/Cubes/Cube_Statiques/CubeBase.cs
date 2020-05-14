using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBase : CubeStatic
{
    [Header("Options des couleurs au sol")]
    public int colorPotential = 0;
    private int initColorPotential;
    public Color color = Color.white;
    private Color initColor;
    public ParticleSystem takeColor = null;
    void Awake()
    {
        cubeType = CubeType.Base;
    }

    
}
