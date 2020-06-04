﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CubeMovable
{
    public bool isEnemyMirror = false;
    public bool isEnemyMoving = false;

    [SerializeField] public bool InvertXAxis = false;
    [SerializeField] public bool InvertZAxis = false;
    
    [HideInInspector] public Color initColor;

    public List<moveEnum> MoveList = new List<moveEnum>();
    protected int CurrentMove = 0;
    protected int CurrentMoveProject = 0;
    protected bool revertMove = false;
    protected bool revertMoveProject = false;

    [SerializeField] private GameObject projection = null;
    [SerializeField] private float projectTimer = 2.5f;

    private bool isProjecting = false;
    private bool isProjectionMove = false;

    private Vector3 projectionPos = Vector3.zero;

    [SerializeField] private float projectionSpeed = 0.8f;

    private float timerTime = 0f;

    public override void OnAwake()
    {
        base.OnAwake();

        SetModeVoid();

        initColor = color;

        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnResetLevel += ResetCube;

        if (isEnemyMirror || isEnemyMoving)Player.OnMove += SetModeMove;
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    public override void StartMoveBehavior()
    {
        base.StartMoveBehavior();

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeDestructible>())
            {
                CubeDestructible tmpCube = hit.transform.gameObject.GetComponent<CubeDestructible>();
                tmpCube.Crumble();


            }
        }
    }

    public override void EndMoveBehavior()
    {
        SetModeVoid();

        transform.eulerAngles = Vector3.zero;

        TestTile();
    }

    public override void SetModeVoid()
    {
        base.SetModeVoid();

        if (isEnemyMoving)
        {
            projection.SetActive(true);
            isProjecting = false;
            timerTime = 0;
        }
    }

    protected override void DoActionVoid()
    {
        base.DoActionVoid();

        if (isEnemyMoving && !isProjecting)
        {
            timerTime += Time.deltaTime;

            if(timerTime >= projectTimer)
            {
                isProjecting = true;
            }
        }

        if (isProjecting)
        {
            if (!isProjectionMove)
            {
                if (CurrentMoveProject == MoveList.Count && !revertMoveProject)
                {
                    if (projection.transform.position == initialPosition)
                    {
                        CurrentMoveProject = 0;
                    }
                    else revertMoveProject = true;
                }
                else if (CurrentMoveProject == 0 && revertMoveProject) revertMoveProject = false;

                if (revertMoveProject) CurrentMoveProject--;

                if ((int)MoveList[CurrentMoveProject] == 0)
                {
                    orientation = Vector3.forward;
                    if (revertMoveProject) orientation = Vector3.back;
                }
                else if ((int)MoveList[CurrentMoveProject] == 1)
                {
                    orientation = Vector3.back;
                    if (revertMoveProject) orientation = Vector3.forward;
                }
                else if ((int)MoveList[CurrentMoveProject] == 2)
                {
                    orientation = Vector3.right;
                    if (revertMoveProject) orientation = Vector3.left;
                }
                else if ((int)MoveList[CurrentMoveProject] == 3)
                {
                    orientation = Vector3.left;
                    if (revertMoveProject) orientation = Vector3.right;
                }
                else
                {
                    if (!revertMoveProject) CurrentMoveProject++;
                    return;
                }

                if (!revertMoveProject) CurrentMoveProject++;

                direction = projection.transform.position + orientation;
                isProjectionMove = true;
                projectionPos = projection.transform.position;
            }
            else if(isProjectionMove) MoveProjection();
        }
    }

    public override void SetModeMove(Vector3 vector)
    {
        if (DoAction == DoActionFall) return;

        if (isEnemyMoving) projection.SetActive(false);

        StartMoveBehavior();

        projection.transform.localPosition = Vector3.zero;

        if (isEnemyMirror)
        {
            if (InvertZAxis)
            {
                if (vector == Vector3.forward || vector == Vector3.back)
                {
                    vector *= -1;
                    orientation = vector;
                }
            }
            else
            {
                orientation = vector;
            }

            if (InvertXAxis)
            {
                if (vector == Vector3.right || vector == Vector3.left)
                {
                    vector *= -1;
                    orientation = vector;
                }
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
        }
        else if (isEnemyMoving)
        {
            if (CurrentMove == MoveList.Count && !revertMove)
            {
                if (transform.position == initialPosition)
                {
                    CurrentMove = 0;
                }
                else revertMove = true;
            }
            else if (CurrentMove == 0 && revertMove) revertMove = false;

            if (revertMove) CurrentMove--;

            if ((int)MoveList[CurrentMove] == 0)
            {
                orientation = Vector3.forward;
                if (revertMove) orientation = Vector3.back;
            }
            else if ((int)MoveList[CurrentMove] == 1)
            {
                orientation = Vector3.back;
                if (revertMove) orientation = Vector3.forward;
            }
            else if ((int)MoveList[CurrentMove] == 2)
            {
                orientation = Vector3.right;
                if (revertMove) orientation = Vector3.left;
            }
            else if ((int)MoveList[CurrentMove] == 3)
            {
                orientation = Vector3.left;
                if (revertMove) orientation = Vector3.right;
            }
            else
            {
                if (!revertMove) CurrentMove++;
                return;
            }

            if (!revertMove) CurrentMove++;

            CurrentMoveProject = CurrentMove;
            revertMoveProject = revertMove;
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

    protected override void DoActionMove()
    {
        base.DoActionMove();

        TestPlayer();
    }

    private void MoveProjection()
    {
        _elapsedTime += Time.deltaTime;

        float ratio = _elapsedTime / projectionSpeed;

        projection.transform.position = Vector3.Lerp(projectionPos, direction, ratio);

        if (_elapsedTime >= projectionSpeed)
        {
            // end move
            _elapsedTime = 0;
            isProjectionMove = false;
        }

    }

    public override void DoActionFall()
    {
        base.DoActionFall();

        if (transform.position.y < initialPosition.y - 1)
        { 
            Explode();

            SetModeVoid();
        }
    }

    public override void TestTile()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<CubeDash>())
            {
                CubeDash tmpCube = hit.transform.gameObject.GetComponent<CubeDash>();

                if ((int)tmpCube.dashOrientation == 0)
                {
                    orientation = Vector3.forward;
                }
                else if ((int)tmpCube.dashOrientation == 1)
                {
                    orientation = Vector3.back;
                }
                else if ((int)tmpCube.dashOrientation == 2)
                {
                    orientation = Vector3.right;
                }
                else
                {
                    orientation = Vector3.left;
                }

                SetModeDash();
            }
            else if (hit.transform.gameObject.GetComponent<CubeDetonator>())
            {
                CubeDetonator tmpCube = hit.transform.gameObject.GetComponent<CubeDetonator>();

                tmpCube.ActivateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeTeleporter>())
            {
                CubeTeleporter tmpCube = hit.transform.gameObject.GetComponent<CubeTeleporter>();

                gameObject.transform.position = new Vector3(teleportDestination.transform.position.x, teleportDestination.transform.position.y + 1f, teleportDestination.transform.position.z);
            }
        }
        else
        {
            DoAction = DoActionFall;
        }
    }

    public override bool TestWall()
    {
        Ray ray = new Ray(transform.position, orientation);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {

            if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                return true;
            }
            
        }

        return false;
    }

    private Vector3 boxSize = new Vector3(0.1f, 0.1f, 0.1f);

    public void TestPlayer()
    {

        RaycastHit hit;

        if (Physics.BoxCast(transform.position, boxSize, orientation, out hit, Quaternion.identity, 0.2f))
        {

            if (hit.transform.parent.gameObject.GetComponent<Player>())
            {
                if(hit.transform.gameObject.GetComponent<Renderer>().material.color == color)
                {
                    Explode();
                }
                else
                {
                    Player tmpPlayer = hit.transform.parent.gameObject.GetComponent<Player>();

                    tmpPlayer.SetDeath();
                }
            }

        }
    }

    [Header("Stretch")]

    public AnimationCurve stretchCube;
    public float stretchSpeed = 1f;
    private float stretchLerp = 0;
    private Vector3 stretchInitPos;

    public void SetModeStretch()
    {
        stretchInitPos = transform.localPosition;
        stretchLerp = 0;
        DoAction = DoStretchCube;

    }

    public void DoStretchCube()
    {
        stretchLerp += Time.deltaTime * stretchSpeed;

        CubeStretch(stretchCube.Evaluate(stretchLerp), stretchInitPos);

        if (stretchLerp >= 1f)
        {
            SetModeVoid();
        }
    }

    public void CubeStretch(float y, Vector3 _initPos)
    {
        float x, z;
        z = x = Mathf.Sqrt(1 / y);

        transform.localScale = new Vector3(x, y, z);
        transform.localPosition = _initPos + (((y - 1) / 2) * Vector3.up);
    }

    public override void Explode()
    {
        if (isEnemyMirror || isEnemyMoving) Player.OnMove -= SetModeMove;

        GameManager.instance.KillEnnemy();

        base.Explode();
    }
}
