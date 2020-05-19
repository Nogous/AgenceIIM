using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePaint : CubeStatic
{
    [Header("Options des couleurs au sol")]
    [SerializeField] private Color color = Color.white;
    public ParticleSystem takeColor = null;
    public override void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        cubeType = CubeType.Paint;
        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    public Color GetPaintColor()
    {
        return color;
    }

    
}
