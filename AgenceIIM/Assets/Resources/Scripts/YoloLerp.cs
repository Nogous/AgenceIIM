using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoloLerp : MonoBehaviour
{
    public GameObject obj;
    public float speed = .1f;

    public GameObject p1;
    public GameObject p2;
    public GameObject angle;

    float lerpAdvencement = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lerpAdvencement += Time.deltaTime * speed;

        Vector3 p1Cur = Vector3.Lerp(p1.transform.position, angle.transform.position, lerpAdvencement);
        Vector3 p2Cur = Vector3.Lerp(angle.transform.position, p2.transform.position, lerpAdvencement);

        obj.transform.position = Vector3.Lerp(p1Cur, p2Cur, lerpAdvencement);

        if (lerpAdvencement > 1)
        {
            lerpAdvencement = 0;
        }
    }
}
