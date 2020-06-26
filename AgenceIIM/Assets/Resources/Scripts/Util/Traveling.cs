using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveling : MonoBehaviour
{
    public Transform ObjToMove;

    public Transform initPos;
    public Transform endPos;
    public float speed;

    public bool playOnStart = false;
    public KeyCode keyStart = KeyCode.Space;

    public bool canDoAgain;
    private bool isTraveling = false;
    private float lerpCount = 0f;

    private void Start()
    {
        ObjToMove.position = initPos.position;
        if (playOnStart)
        StartTravel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyStart))
        {
            StartTravel();
        }

        if (isTraveling)
        {
            DoTraveling();
        }
    }

    public void StartTravel()
    {
        if (isTraveling && !canDoAgain) return;

        isTraveling = true;
        lerpCount = 0f;
    }

    private void DoTraveling()
    {
        lerpCount += Time.deltaTime * speed;
        ObjToMove.position = Vector3.Lerp(initPos.position, endPos.position, lerpCount);
    }
}
