using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CubeType
{
    Base,
    EnnemiStatique,
    EnnemiPattern,
    EnnemiMiroir,
    Peinture,
    Cleaner,
    ArcEnCiel,
    Téléporteur,
    Dash,
    Glissant,
    Mur,
    TNT,
    Interrupteur,
    Destructible,
    BlocMouvant,
}

public class Level : MonoBehaviour
{
    public Vector2 levelSize = new Vector2(15, 15);
    public List<CubeData> cubeDatas = new List<CubeData>();
    public List<GameObject> cubes = new List<GameObject>();

    public CubeType cubeType;
    public GameObject cubePrefab = null;

    public GameObject player;

    public void SetupCube(CubeType _cubeType, Vector3 pos)
    {
        GameObject obj = null;

        for (int i = cubes.Count; i-->0;)
        {
            if(cubes[i].name == pos.ToString())
            {
                obj = cubes[i];
                break;
            }
        }

        if (obj == null)
        {
            obj = Instantiate(cubePrefab);
            obj.name = pos.ToString();
            obj.transform.parent = transform;
            cubes.Add(obj);
        }

        obj.transform.position = pos;

        Cube cubeObj = obj.GetComponent<Cube>();
        if (cubeObj != null)
        {
            switch (_cubeType)
            {
                case CubeType.Base:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.EnnemiStatique:
                    //cubeObj.SetEnemy();
                    break;
                case CubeType.EnnemiPattern:
                    //cubeObj.SetEnemyMoving();
                    break;
                case CubeType.EnnemiMiroir:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Peinture:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Cleaner:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.ArcEnCiel:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Téléporteur:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Dash:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Glissant:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Mur:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.TNT:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Interrupteur:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.Destructible:
                    cubeObj.SetCubeBase();
                    break;
                case CubeType.BlocMouvant:
                    cubeObj.SetCubeBase();
                    break;
                default:
                    cubeObj.SetCubeBase();
                    break;
            }
        }
    }
}
