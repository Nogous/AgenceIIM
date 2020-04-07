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
    public Player player;
    public Transform focusBottom;

    public void Update()
    {
        TestGround();
    }

    public void TestGround()
    {
        Ray ray = new Ray(player.transform.position, focusBottom.localPosition);
        Debug.DrawRay(player.transform.position, focusBottom.localPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit ,1f))
        {
            if (hit.transform.gameObject.CompareTag("Color"))
            {
                player.faceColor[1].GetComponent<Renderer>().material.color = hit.transform.gameObject.GetComponent<Renderer>().material.color;
            }
        }
    }
}
