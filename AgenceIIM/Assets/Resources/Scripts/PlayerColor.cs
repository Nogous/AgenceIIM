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
    public static PlayerColor instance = null;

    public Player player;
    public Transform focusBottom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Update()
    {
        //TestGround();
    }

    public void TestGround()
    {
        Ray ray = new Ray(player.transform.position, focusBottom.localPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit ,1f))
        {
            if (hit.transform.gameObject.CompareTag("Color"))
            {
                player.faceColor[1].GetComponent<Renderer>().material.color = hit.transform.gameObject.GetComponent<Renderer>().material.color;
            }
        }
    }

    public void TestNextTile(MoveDir moveDir)
    {
        Ray ray = new Ray(player.transform.position, Vector3.forward);

        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(player.transform.position, Vector3.back);
                break;
            case MoveDir.right:
                ray = new Ray(player.transform.position, Vector3.right);
                break;
            case MoveDir.left:
                ray = new Ray(player.transform.position, Vector3.left);
                break;
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.black, 1f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.gameObject.CompareTag("Color"))
            {
                player.faceColor[1].GetComponent<Renderer>().material.color = hit.transform.gameObject.GetComponent<Renderer>().material.color;
            }
        }
    }
}
