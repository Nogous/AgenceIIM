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
    public CubeType cubeType;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    //Valeurs évolutives
    public Vector3 orientation = Vector3.forward;
    protected Vector3 direction;
    protected Vector3 axis = Vector3.right;
    protected List<Vector3> vectors = new List<Vector3>();
    protected Quaternion addedRotation;
    protected Vector3 previousPos;
    protected Vector3 previousOr;
    protected Quaternion previousRot;
    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        vectors.Add(Vector3.forward);
        vectors.Add(Vector3.back);
        vectors.Add(Vector3.left);
        vectors.Add(Vector3.right);
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

    public void SetCubeBase()
    {

    }

    public void SetTexture(Material mat)
    {
        gameObject.GetComponent<Renderer>().sharedMaterial = mat;
    }

    protected void TestTile()
    {
        // test tile d'arriver
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                if (tmpCube.GetType() == typeof(CubeDash))
                {
                    CubeDash tmpDash = (CubeDash)tmpCube;
                    if (tmpDash.dashOrientation == CubeDash.dashEnum.forward)
                    {
                        orientation = Vector3.forward;
                    }
                    else if (tmpDash.dashOrientation == CubeDash.dashEnum.backward)
                    {
                        orientation = Vector3.back;
                    }
                    else if (tmpDash.dashOrientation == CubeDash.dashEnum.right)
                    {
                        orientation = Vector3.right;
                    }
                    else
                    {
                        orientation = Vector3.left;
                    }

                    SetModeDash();
                }
                else if (tmpCube.GetType() == typeof(CubeDetonator))
                {
                    CubeDetonator tmpDetonator = (CubeDetonator)tmpCube; 
                    tmpDetonator.ActivateTnt();
                }
                else if (tmpCube.GetType() == typeof(CubeTeleporter))
                {
                    CubeTeleporter tmpTeleporter = (CubeTeleporter)tmpCube;
                    gameObject.transform.position = new Vector3(tmpTeleporter.transform.position.x, tmpTeleporter.transform.position.y + 1f, tmpTeleporter.transform.position.z);
                }

            }

        }
        else
        {
            DoAction = DoActionFall;
        }
    }
}
