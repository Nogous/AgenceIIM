using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDash : CubeStatic
{
    public enum dashEnum
    {
        forward,
        backward,
        right,  
        left, 
    };
    private float _elapsedTime = 0;
    private static float diagonal = 1 * Mathf.Sqrt(2);
    private float offset = (diagonal - 1) / 2;

    [Header("Options du dash")]
    [SerializeField] public dashEnum dashOrientation = dashEnum.forward;
    [SerializeField] private float _moveTime = 0.2f;
    void Awake()
    {
        cubeType = CubeType.Dash;
    }

    private void SetModeDash()
    {
        RotationCheck();

        _elapsedTime = 0;

        direction = transform.position + orientation * 2;
        previousRot = transform.rotation;
        addedRotation = previousRot * Quaternion.AngleAxis(90f, axis);
        previousPos = transform.position;

        DoAction = DoActionDash;
    }

    private void DoActionDash()
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

            TestTile();
        }
    }

    private void RotationCheck()
    {
        if (orientation == Vector3.forward) axis = Vector3.right;
        else if (orientation == Vector3.right) axis = Vector3.back;
        else if (orientation == Vector3.back) axis = Vector3.left;
        else axis = Vector3.forward;
    }

}
