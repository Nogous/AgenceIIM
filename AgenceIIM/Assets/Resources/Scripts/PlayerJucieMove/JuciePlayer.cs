using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    Up,
    Down,
    Right,
    Left,
}

public class JuciePlayer : MonoBehaviour
{
    public float speedMove = 1f;

    private bool isMoving;
    private float lerpMove = 0f;

    private Vector3 initPosLerp;
    private Vector3 endPosLerp;

    private Vector3 initAnimPos;

    [Range(0,1)]
    public float i = 1f;
    
    void Awake()
    {
        isMoving = false;
        initAnimPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
        else
        {

        }

        MoveShape(i);
    }

    public void Move()
    {
        lerpMove += Time.deltaTime * speedMove;
        transform.position = Vector3.Lerp(initPosLerp, endPosLerp, lerpMove);

        if (lerpMove >= 1f)
        {
            isMoving = false;
        }
    }

    public void SetMove(Dir dir)
    {
        lerpMove = 0f;
        initPosLerp = transform.position;

        switch (dir)
        {
            case Dir.Up:
                endPosLerp = initPosLerp + Vector3.forward;
                break;
            case Dir.Down:
                endPosLerp = initPosLerp + Vector3.back;
                break;
            case Dir.Right:
                endPosLerp = initPosLerp + Vector3.right;
                break;
            case Dir.Left:
                endPosLerp = initPosLerp + Vector3.left;
                break;
        }

        isMoving = true;
    }

    public void MoveShape(float y)
    {
        float x, z;
        z = x = Mathf.Sqrt(1 / y);

        transform.localScale = new Vector3(x, y, z);
        transform.localPosition = initAnimPos + (((y - 1) / 2 ) * Vector3.up);
    }
}
