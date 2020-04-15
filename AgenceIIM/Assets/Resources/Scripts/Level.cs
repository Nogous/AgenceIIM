using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject cubePrefab = null;
    public int length = 15;
    public int height = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateCube()
    {
        if (cubePrefab != null)
        {
            return Instantiate(cubePrefab);
        }
        return null;
    }
}
