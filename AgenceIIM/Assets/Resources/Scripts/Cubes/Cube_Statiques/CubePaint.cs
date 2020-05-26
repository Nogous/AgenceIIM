using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePaint : CubeStatic
{
    [Header("Options des couleurs au sol")]
    [SerializeField] private Color color = Color.white;
    public ParticleSystem takeColor = null;
    public float colorPotencial = 100;
    public float initColorPotencial;

    public GameObject mask;
    public Vector3 initalScale;

    public override void OnAwake()
    {
        base.OnAwake();
        initalScale = mask.transform.localScale;
        initColorPotencial = colorPotencial;

        isBreakable = false;

        cubeType = CubeType.Paint;
        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }

        ParticleSystem.MainModule mainMod = GetComponentInChildren<ParticleSystem>().main;

        mainMod.startColor = color;
    }

    public Color GetPaintColor()
    {
        return color;
    }

    public override void ResetCube()
    {
        base.ResetCube();

        mask.transform.localScale = initalScale;
        colorPotencial = initColorPotencial;
    }
}
