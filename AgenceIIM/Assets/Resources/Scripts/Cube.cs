using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [Header("General Settings")]

    public bool isEnemy = false;
    public bool isEnemyMoving = false;
    public bool isCleaningBox = false;
    public bool isDashBox = false;
    public bool isWall = false;

    public bool isTnt = false;
    public bool isTrigger = false;

    [SerializeField] private Cube associatedTnt = null;

    public enum dashEnum
    {
        forward,
        backward,
        right,  
        left, 
    };

    [Header("Dash Settings")]

    [SerializeField] public dashEnum dashOrientation = dashEnum.forward;

    [Header("Enemy Settings")]

    public Color enemyColor;

    [SerializeField] public bool InvertXAxis = false;
    [SerializeField] public bool InvertZAxis = false;

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

    private Action DoAction;

    [Header("Movement Settings")]

    [SerializeField] private float _moveTime = 0.2f;

    private float _elapsedTime = 0;
    public Rewired.Player replayer;
    private Vector3 direction;
    public Vector3 orientation = Vector3.forward;
    private Vector3 axis = Vector3.right;

    private Quaternion addedRotation;
    private Vector3 previousPos;
    private Vector3 previousOr;
    private Quaternion previousRot;

    private Quaternion zeroRot = new Quaternion(0, 0, 0, 0);

    private static float diagonal = 1 * Mathf.Sqrt(2);
    private float offset = (diagonal - 1) / 2;
    public float speed = 5;

    // Start is called before the first frame update
    private void Awake()
    {
        initPos = transform.position;
        initRot = transform.rotation;
        initColorPotencial = colorPotencial;
        initColor = color;
        enemyColor = color;

        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }

    }

    private void SetModeVoid()
    {
        DoAction = DoActionVoid;
    }

    private void DoActionVoid()
    {

    }

    private void Start()
    {
        GameManager.instance.AddCube(this);

        if (!isEnemy && !isEnemyMoving)
        {
            if (stain != null)
            {
                stainScale = stain.transform.localScale;
                stainColor = stain.GetComponent<Renderer>().material.color;
            }
        }

        if (isEnemyMoving) Player.OnMove += SetModeMove;

        if (isBreakable)
        {
            cubesPivotDistance = cubeSize * cubesInRow / 2;
            cubesPivot = Vector3.one * cubesPivotDistance;
        }

        SetModeVoid();
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

    [Header("Explosion Settings")]

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
            AudioManager.instance.Play("Splash");
            AudioManager.instance.Play("ExplosionCube");
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

    private void Update()
    {
        DoAction();
    }

    private void SetModeMove(Vector3 vector)
    {
        if (InvertZAxis)
        {
            if (vector == Vector3.forward || vector == Vector3.back) orientation = vector * -1;
        }
        else
        {
            orientation = vector;
        }

        if (InvertXAxis)
        {
            if (vector == Vector3.right || vector == Vector3.left) orientation = vector * -1;
        }
        else
        {
            orientation = vector;
        }

        if (TestWall())
        {
            SetModeVoid();
            return;
        }  

        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        // init move

        DoAction = DoActionMove;
    }

    private void DoActionMove()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        transform.position = new Vector3(transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            // end move
            SetModeVoid();

            transform.eulerAngles = Vector3.zero;
        }
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;
    }

    private bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {

            if (hit.transform.gameObject.GetComponent<Cube>())
            {

                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                if (tmpCube.isWall) return true;
            }
        }

        return false;
    }

    public void ActivateTnt()
    {
        if(associatedTnt != null)
        {
            associatedTnt.Explode();
        }
    }
}
