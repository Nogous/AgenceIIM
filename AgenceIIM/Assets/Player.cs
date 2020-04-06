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

    // Start is called before the first frame update
    void Start()
    {
        SetModeWait();
    }

    private void SetModeWait()
    {
        DoAction = DoActionWait;
    }

    private void DoActionWait()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            orientation = Vector3.forward;
            SetModeMove();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            orientation = Vector3.back;
            SetModeMove();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            orientation = Vector3.right;
            SetModeMove();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
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

        transform.rotation = zeroRot;

        direction = transform.position + orientation;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        DoAction = DoActionMove;
    }

    private void DoActionMove()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / _moveTime;

        transform.position = Vector3.Lerp(previousPos, direction, ratio);

        transform.rotation = Quaternion.Lerp(previousRot, addedRotation, ratio);

        transform.position = new Vector3(transform.position.x, previousPos.y + Mathf.Clamp(Mathf.Sin(ratio * Mathf.PI) * offset, 0, 1), transform.position.z);

        if (_elapsedTime >= _moveTime) SetModeWait();
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;

    }
}
