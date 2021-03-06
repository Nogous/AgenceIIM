﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CubeType
{
    NoCube,
    Base,
    EnnemiStatique,
    EnnemiPattern,
    EnnemiMiroir,
    Paint,
    Cleaner,
    ArcEnCiel,
    Teleporter,
    Dash,
    Glissant,
    Mur,
    TNT,
    Detonator,
    Destructible,
    BlocMouvant,
    Wall,
}
public class Cube : MonoBehaviour
{
    //Valeurs de début
    protected CubeType cubeType = CubeType.Base;
    protected Vector3 initialPosition;
    protected Quaternion initialRotation;

    public void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Start()
    {
        GameManager.instance.OnResetLevel += ResetCube;
    }

    public virtual void OnStart()
    {
        GameManager.instance.OnResetLevel += ResetCube;
    }

    public virtual void ResetCube()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        gameObject.SetActive(true);
    }

    public CubeType GetCubeType()
    {
        return cubeType;
    }
}
