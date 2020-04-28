using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStructure
{

}

public class SpawnLevel : MonoBehaviour
{
    public List<GameObject> cubes = new List<GameObject>();

    public float floatLerp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play("");
    }

    //public void

    public void UpdateFallLevel()
    {
        floatLerp = 0f;

    }
}
