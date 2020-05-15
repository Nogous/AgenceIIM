using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWall : CubeStatic
{
    void Awake()
    {
        cubeType = CubeType.Wall;
    }

    public bool TestWall()
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

}