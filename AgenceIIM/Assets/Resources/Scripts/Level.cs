﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject cubePrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCube(Vector3 pos)
    {
        if (cubePrefab != null)
        {
            Instantiate(cubePrefab);
        }
    }
}
