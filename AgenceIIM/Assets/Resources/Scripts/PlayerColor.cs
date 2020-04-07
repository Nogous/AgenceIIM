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
        Color tmpColor = player.faceColor[4].GetComponent<Renderer>().material.color;

        Ray rayBottom = new Ray(player.transform.position, focusBottom.localPosition);
        RaycastHit hitBottom;

        if (Physics.Raycast(rayBottom, out hitBottom, 1f))
        {
            hitBottom.transform.gameObject.SetActive(false);
        }

        switch (moveDir)
        {
            case MoveDir.down:
                ray = new Ray(player.transform.position, Vector3.back);
                tmpColor = player.faceColor[3].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.right:
                ray = new Ray(player.transform.position, Vector3.right);
                tmpColor = player.faceColor[2].GetComponent<Renderer>().material.color;
                break;
            case MoveDir.left:
                ray = new Ray(player.transform.position, Vector3.left);
                tmpColor = player.faceColor[5].GetComponent<Renderer>().material.color;
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
            }
        }
    }
}
