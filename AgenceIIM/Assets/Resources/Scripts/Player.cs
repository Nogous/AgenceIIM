using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDir
{
    up,
    down,
    right,
    left,
}

public class Player : MonoBehaviour
{
    private float _elapsedTime = 0;

    [SerializeField] private float _moveTime = 0.2f;

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

    private Action DoAction;

    [SerializeField] private KeyCode up = KeyCode.Z;
    [SerializeField] private KeyCode down = KeyCode.S;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode left = KeyCode.Q;

    [SerializeField] private GameObject Cube = null;

    private Quaternion newCubeRot;


    [SerializeField] private Renderer[] faceColor = new Renderer[6];
    private Color[] initColor = new Color[6];
    private Vector3 initPos;
    private MoveDir lastMove;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.player == null)
        {
        GameManager.instance.player = this;
        }

        // initialisation depart
        for (int i = faceColor.Length; i-- > 0;)
        {
            initColor[i] = faceColor[i].material.color;
        }
        initPos = transform.position;

        // start move
        SetModeWait();
    }

    public void ResetPlayer()
    {
        for (int i = faceColor.Length; i-- > 0;)
        {
            faceColor[i].material.color = initColor[i];
        }
        transform.position = initPos;
        SetModeWait();
    }

    private void SetModeWait()
    {
        DoAction = DoActionWait;
    }

    private void DoActionWait()
    {
        if (Input.GetKey(up))
        {
            lastMove = MoveDir.up;
            orientation = Vector3.forward;
            SetModeMove();
            TestNextTile(MoveDir.up);
        }
        else if (Input.GetKey(down))
        {
            lastMove = MoveDir.down;
            orientation = Vector3.back;
            SetModeMove();
            TestNextTile(MoveDir.down);
        }
        else if (Input.GetKey(right))
        {
            lastMove = MoveDir.right;
            orientation = Vector3.right;
            SetModeMove();
            TestNextTile(MoveDir.right);
        }
        else if (Input.GetKey(left))
        {
            lastMove = MoveDir.left;
            orientation = Vector3.left;
            SetModeMove();
            TestNextTile(MoveDir.left);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    private void SetModeMove()
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation;
        previousRot = Cube.transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        DoAction = DoActionMove;
    }

    private void DoActionMove()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        Cube.transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        transform.position = new Vector3(transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), transform.position.z);

        if (_elapsedTime >= _moveTime)
        {
            SetModeWait();

            Cube.transform.eulerAngles = Vector3.zero;
            UpdateColor(lastMove);

            TestGround();
        }
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;

    }


    private void UpdateColor(MoveDir moveDir)
    {
        Color tmpColor = faceColor[0].material.color;

        switch (moveDir)
        {
            case MoveDir.down:
                faceColor[0].material.color = faceColor[4].material.color;
                faceColor[4].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[3].material.color;
                faceColor[3].material.color = tmpColor;
                break;
            case MoveDir.up:
                faceColor[0].material.color = faceColor[3].material.color;
                faceColor[3].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[4].material.color;
                faceColor[4].material.color = tmpColor;
                break;
            case MoveDir.left:
                faceColor[0].material.color = faceColor[2].material.color;
                faceColor[2].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[5].material.color;
                faceColor[5].material.color = tmpColor;
                break;
            case MoveDir.right:
                faceColor[0].material.color = faceColor[5].material.color;
                faceColor[5].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[2].material.color;
                faceColor[2].material.color = tmpColor;
                break;
        }
    }


    private void TestGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.CompareTag("Color"))
            {
                faceColor[1].GetComponent<Renderer>().material.color = hit.transform.gameObject.GetComponent<Renderer>().material.color;
            }
        }
    }

    private void TestNextTile(MoveDir moveDir)
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        Color tmpColor = faceColor[4].GetComponent<Renderer>().material.color;

        Ray rayBottom = new Ray(transform.position, Vector3.down);
        RaycastHit hitBottom;

        if (Physics.Raycast(rayBottom, out hitBottom, 1f))
        {
            hitBottom.transform.gameObject.SetActive(false);
        }

        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(transform.position, Vector3.back);
                tmpColor = faceColor[3].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.right:
                ray = new Ray(transform.position, Vector3.right);
                tmpColor = faceColor[2].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.left:
                ray = new Ray(transform.position, Vector3.left);
                tmpColor = faceColor[5].GetComponent<Renderer>().material.color;
                break;
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.black, 1f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                if (tmpColor == hit.transform.gameObject.GetComponent<Renderer>().material.color)
                {
                    hit.transform.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("You lose");
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
