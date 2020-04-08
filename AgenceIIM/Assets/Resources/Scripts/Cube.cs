using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Vector3 initPos;
    private Quaternion initRot;

    [SerializeField] private bool isEnemy = false;

    [SerializeField] private bool isBreakable = false;

    [SerializeField] private int colorPotencial = 0;
    private int initColorPotencial;
    [SerializeField] private Color color = Color.white;
    private Color initColor;

    [SerializeField] private GameObject stain = null;

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

        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    private void Start()
    {
        GameManager.instance.AddCube(this);
    }

    public void ResetCube()
    {
        transform.position = initPos;
        transform.rotation = initRot;
        colorPotencial = initColorPotencial;
        color = initColor;
    }

    public void ActivateStain(Color tint)
    {
        if(colorPotencial > 0)
        {
            stain.SetActive(true);
            stain.GetComponent<Renderer>().material.color = tint;
        }
    }

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
        piece.transform.position = transform.position + new Vector3(cubeSize *x, cubeSize * y, cubeSize * z);
        piece.transform.localScale = Vector3.one * cubeSize;

        // add rigidbody and mass
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = cubeSize;

        Destroy(piece, 1.5f);
    }

    #endregion
}
