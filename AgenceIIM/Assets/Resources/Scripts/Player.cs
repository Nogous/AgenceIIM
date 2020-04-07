using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private GameObject Cube;

    private Quaternion newCubeRot;

    // Start is called before the first frame update
    void Start()
    {
        SetModeWait();

        faceColor[0].material.color = Color.red;
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
        }
        else if (Input.GetKey(down))
        {
            lastMove = MoveDir.down;
            orientation = Vector3.back;
            SetModeMove();
        }
        else if (Input.GetKey(right))
        {
            lastMove = MoveDir.right;
            orientation = Vector3.right;
            SetModeMove();
        }
        else if (Input.GetKey(left))
        {
            lastMove = MoveDir.left;
            orientation = Vector3.left;
            SetModeMove();
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
        }
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;

    }

    public Renderer[] faceColor;
    public MoveDir lastMove;

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
}
