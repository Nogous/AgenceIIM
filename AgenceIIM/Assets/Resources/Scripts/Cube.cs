using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Vector3 initPos;
    private Quaternion initRot;

    // Start is called before the first frame update
    private void Awake()
    {
        initPos = transform.position;
        initPos.x = (int)initPos.x;
        initPos.y = (int)initPos.y;
        initPos.z = (int)initPos.z;

        transform.position = initPos;
        initRot = transform.rotation;
    }

    private void Start()
    {
        GameManager.instance.AddCube(this);
    }

    public void ResetCube()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }
}
