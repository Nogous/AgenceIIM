using System.Collections;
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
    Peinture,
    Cleaner,
    ArcEnCiel,
    Teleporteur,
    Dash,
    Glissant,
    Mur,
    TNT,
    Interrupteur,
    Destructible,
    BlocMouvant,
}
public class Cube : MonoBehaviour
{
    public CubeType cubeType;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetCube()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    public CubeType GetCubeType()
    {
        return cubeType;
    }
}
