using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Level : MonoBehaviour
{
    [HideInInspector] public string nameLevel = "";

    [HideInInspector] public Vector2 levelSize = new Vector2(15, 15);
    [HideInInspector] public List<CubeData> cubeDatas = new List<CubeData>();
    [HideInInspector] public List<GameObject> cubes = new List<GameObject>();

    [HideInInspector] public CubeType cubeType;
    [HideInInspector] public GameObject cubePrefab = null;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Transform cubeBox;

    #region texture
    [Header("no Cube")]
    public Texture2D texture2DNoCube;
    [Header("Cube Base")]
    public Texture2D texture2DCubeBase;
    public Material matCubeBase = null;
    [Header("EnnemiStatique")]
    public Texture2D texture2DEnnemiStatique;
    public Material matEnnemiStatique = null;
    [Header("EnnemiPattern")]
    public Texture2D texture2DEnnemiPattern;
    public Material matEnnemiPattern = null;
    [Header("EnnemiMiroir")]
    public Texture2D texture2DEnnemiMiroir;
    public Material matEnnemiMiroir = null;
    [Header("Cube Peinture")]
    public Texture2D texture2DCubePeinture;
    public Material matCubePeinture = null;
    [Header("Cube Cleaner")]
    public Texture2D texture2DCubeCleaner;
    public Material matCubeCleaner = null;
    [Header("Cube ArcEnCiel")]
    public Texture2D texture2DCubeArcEnCiel;
    public Material matCubeArcEnCiel = null;
    [Header("Cube Dash")]
    public Texture2D texture2DCubeDash;
    public Material matCubeDash = null;
    [Header("Cube Téléporteur")]
    public Texture2D texture2DCubeTeleporteur;
    public Material matCubeTeleporteur = null;
    [Header("Cube Glissant")]
    public Texture2D texture2DCubeGlissant;
    public Material matCubeGlissant = null;
    [Header("Cube Mur")]
    public Texture2D texture2DCubeMur;
    public Material matCubeMur = null;
    [Header("Cube TNT")]
    public Texture2D texture2DCubeTNT;
    public Material matCubeTNT = null;
    [Header("Cube Interrupteur")]
    public Texture2D texture2DCubeInterrupteur;
    public Material matCubeInterrupteur = null;
    [Header("Cube Destructible")]
    public Texture2D texture2DCubeDestructible;
    public Material matCubeDestructible = null;
    [Header("Cube BlocMouvant")]
    public Texture2D texture2DCubeBlocMouvant;
    public Material matCubeBlocMouvant = null;
    #endregion

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
            obj.transform.parent = cubeBox;
            cubes.Add(obj);
        }

        obj.transform.position = pos;

        Cube cubeObj = obj.GetComponent<Cube>();
        if (cubeObj != null)
        {
            switch (_cubeType)
            {
                case CubeType.NoCube:
                    cubeObj.gameObject.SetActive(false);
                    break;
                case CubeType.Base:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.EnnemiStatique:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.EnnemiPattern:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.EnnemiMiroir:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Paint:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Cleaner:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.ArcEnCiel:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Teleporter:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Dash:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Glissant:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Mur:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.TNT:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Detonator:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.Destructible:
                    cubeObj.gameObject.SetActive(true);
                    break;
                case CubeType.BlocMouvant:
                    cubeObj.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
