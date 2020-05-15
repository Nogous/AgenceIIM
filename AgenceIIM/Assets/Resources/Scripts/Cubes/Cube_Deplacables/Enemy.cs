using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CubeMovable
{
    public bool isEnemyMirror = false;
    public bool isEnemyMoving = false;

    [SerializeField] public bool InvertXAxis = false;
    [SerializeField] public bool InvertZAxis = false;

    public Color enemyColor;
    private Color initColor;

    private void Awake()
    {
        SetModeVoid();

        initColor = color;
        enemyColor = color;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (color != Color.white)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
        }

        vectors.Add(Vector3.forward);
        vectors.Add(Vector3.back);
        vectors.Add(Vector3.left);
        vectors.Add(Vector3.right);
    }

    // Start is called before the first frame update
    public override void OnStart()
    {
        base.OnStart();

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
    }

    public override void EndMoveBehavior()
    {
        SetModeVoid();

        transform.eulerAngles = Vector3.zero;

        TestTile();
    }

    public override void SetModeMove(Vector3 vector)
    {
        if (DoAction == DoActionFall) return;

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
            else
            {
                orientation = Vector3.left;
                if (revertMove) orientation = Vector3.right;
            }

            if (!revertMove) CurrentMove++;

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


    public override void DoActionFall()
    {
        base.DoActionFall();

        if (transform.position.y < initialPosition.y - 1)
        {
            if (isEnemyMirror)
            {
                //Explode();
            }

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

                //tmpCube.ActivateTnt();
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

            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                return true;
            }
        }

        return false;
    }

    public override void Explode()
    {
        if (isEnemyMirror || isEnemyMoving) Player.OnMove -= SetModeMove;

        GameManager.instance.KillEnnemy();

        base.Explode();
    }
}
