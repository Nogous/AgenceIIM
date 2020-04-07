using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    bool travel;
    bool position;
    float progress = 0.0f;
    GameObject cameraGO;
    Vector3 positionDépart;
    [Header("Renseignez la position alternative de la caméra")]
    public Vector3 positionAlternatif;
    [Range(2, 100)] 
    public int slowFactor;
    [Range(0.01f, 0.90f)] 
    public float cutoff;
    public void TravelTick()
    {
        travel = true;
    }

    public void Start()
    {
        cameraGO = GameObject.Find("Camera");
        positionDépart = cameraGO.transform.position;
    }
    public void Update()
    {
        if (travel)
        {
            Travel();
        }
        Debug.Log(progress);
    }

    public void Travel()
    {
        if (progress < cutoff)
        {
            switch (position)
            {
                case true:
                {
                    progress += (Time.deltaTime / slowFactor);
                    cameraGO.transform.position = Vector3.Lerp(cameraGO.transform.position, positionDépart, progress);
                }
                break;
                case false:
                {
                    progress += (Time.deltaTime / slowFactor);
                    cameraGO.transform.position = Vector3.Lerp(cameraGO.transform.position, positionAlternatif, progress);
                }
                break;
            }
        }
        else
        {
            if (!position)
            {
                position = true;
                cameraGO.transform.position = positionAlternatif;
                Debug.Log("PosALT");
            }
            else
            {
                position = false;
                cameraGO.transform.position = positionDépart;
                Debug.Log("PosORG");
            }
            progress = 0f;
            travel = false;
            
        }
    }
}

