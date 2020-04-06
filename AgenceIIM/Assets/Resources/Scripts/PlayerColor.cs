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

public class PlayerColor : MonoBehaviour
{
    // controles
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;

    // cube
    public Renderer[] faceColor;
    // top, bottom, left, up, down, right
    //    3
    //  2 0 5 1 
    //    4

    private void Start()
    {
        faceColor[0].material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(up))
        {
            UpdateColor(MoveDir.up);
            //    0
            //  2 4 5 3
            //    1
        }
        if (Input.GetKeyDown(down))
        {
            UpdateColor(MoveDir.down);
            //    1
            //  2 3 5 4
            //    0
        }
        if (Input.GetKeyDown(right))
        {
            UpdateColor(MoveDir.right);
            //    3
            //  1 2 0 5 
            //    4
        }
        if (Input.GetKeyDown(left))
        {
            UpdateColor(MoveDir.left);
            //    3
            //  0 5 1 2 
            //    4
        }
    }
    //    3
    //  2 0 5 1 
    //    4

    public void UpdateColor(MoveDir moveDir)
    {
        Color tmpColor = faceColor[0].material.color;

        switch (moveDir)
        {
            case MoveDir.up:
                faceColor[0].material.color = faceColor[4].material.color;
                faceColor[4].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[3].material.color;
                faceColor[3].material.color = tmpColor;
                break;
            case MoveDir.down:
                faceColor[0].material.color = faceColor[3].material.color;
                faceColor[3].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[4].material.color;
                faceColor[4].material.color = tmpColor;
                break;
            case MoveDir.right:
                faceColor[0].material.color = faceColor[2].material.color;
                faceColor[2].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[5].material.color;
                faceColor[5].material.color = tmpColor;
                break;
            case MoveDir.left:
                faceColor[0].material.color = faceColor[5].material.color;
                faceColor[5].material.color = faceColor[1].material.color;
                faceColor[1].material.color = faceColor[2].material.color;
                faceColor[2].material.color = tmpColor;
                break;
        }
    }
}
