using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("General Settings")]

    public bool isEnemy = false;
    public Color enemyColor;

    private Vector3 initPos;
    private Quaternion initRot;

    [SerializeField] private bool isBreakable = false;

    public int colorPotencial = 0;
    private int initColorPotencial;
    public Color color = Color.white;
    private Color initColor;

    [Header("Effect Settings")]

    [SerializeField] private GameObject stain = null;
    private Vector3 stainScale = Vector3.one;
    private Color stainColor = Color.white;

    [SerializeField] private AnimationCurve fadeCurve = null;
    [SerializeField] private AnimationCurve shrinkCurve = null;

    private float elapsedTime = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        initPos = transform.position;
        initPos.x = (int)initPos.x;
        initPos.y = (int)initPos.y;
        initPos.z = (int)initPos.z;

        transform.position = initPos;
        initRot = transform.rotation;
        initColorPotencial = colorPotencial;
        initColor = color;
        enemyColor = color;

        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    private void Start()
    {
        GameManager.instance.AddCube(this);

        if (!isEnemy)
        {
            if (stain != null)
            {
                stainScale = stain.transform.localScale;
                stainColor = stain.GetComponent<Renderer>().material.color;
            }
        }

        if (isBreakable)
        {
            cubesPivotDistance = cubeSize * cubesInRow / 2;
            cubesPivot = Vector3.one * cubesPivotDistance;
        }
    }

    public void ResetCube()
    {
        transform.position = initPos;
        transform.rotation = initRot;
        colorPotencial = initColorPotencial;
        color = initColor;
    }

    #region Effects

    public void ActivateStain(Color tint)
    {
        if (stain == null) return;
        if (colorPotencial == 0 && !isEnemy)
        {
            stain.SetActive(true);
            stain.GetComponent<Renderer>().material.color = tint;

            stain.transform.localScale = stainScale;
            elapsedTime = 0;

            StopCoroutine(StainRemove());
            StartCoroutine(StainRemove());
        }
    }

    private IEnumerator StainRemove()
    {
        Color colorFade = stain.GetComponent<Renderer>().material.color;
        Vector3 sizeShrink = stainScale;

        while (stain.GetComponent<Renderer>().material.color.a > 0)
        {           
            elapsedTime += Time.deltaTime;

            colorFade.a = fadeCurve.Evaluate(elapsedTime);
            sizeShrink.x = shrinkCurve.Evaluate(elapsedTime);
            sizeShrink.y = shrinkCurve.Evaluate(elapsedTime);

            stain.transform.localScale = sizeShrink;
            stain.GetComponent<Renderer>().material.color = colorFade;

            yield return null;
        }

        StainReset();
    }

    private void StainReset()
    {
        StopCoroutine(StainRemove());

        stain.GetComponent<Renderer>().material.color = stainColor;
        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        stain.SetActive(false);
    }

    #endregion

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.04f;

    #region Explosion

    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private int cubesInRow = 5;

    public void Explode(bool isPlayer = false)
    {
        if (!isPlayer)
        {
            if (!isBreakable) return;

            //make object disappear
            gameObject.SetActive(false);
        }
        else
        {
            AudioManager.instance.Player("Splash");
            AudioManager.instance.Player("ExplosionCube");
        }    

        // loop 3 times to create 5x5x5 pices un x,y,z coordonate
        for (int i = cubesInRow; i-->0;)
        {
            for (int j = cubesInRow; j-- > 0;)
            {
                for (int k = cubesInRow; k-- > 0;)
                {
                    CreatePiece(i, j, k);
                }
            }
        }

        // get explosion position
        Vector3 explosionPos = transform.position;
        // get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // addd explosion force
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    public Color GetColor()
    {
        if (colorPotencial > 0)
        {
            colorPotencial--;
            return color;
        }
        return Color.white;
    }

    private void CreatePiece(int x, int y, int z)
    {
        // create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // set scale and position
        piece.transform.position = transform.position + new Vector3(cubeSize *x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = Vector3.one * cubeSize;

        // add rigidbody and mass
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = cubeSize;

        Destroy(piece, 1.5f);
    }

    #endregion
}
