using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance = null;
    bool travel;
    [System.NonSerialized]
    public bool position;
    float progress = 0.0f;
    public GameObject cameraGO;
    public Vector3 positionDepart;
    public Vector3 positionAlternatif;
    public Vector3 positionTan;
    Vector3 vectorTan;
    Vector3 vectorNormal;
    [Range(2, 100)] 
    public int slowFactor;
    [Range(0.01f, 0.90f)] 
    public float cutoff;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        try
        {
            cameraGO.name = "Camera";
        }
        catch (UnassignedReferenceException)
        {
            cameraGO = GameObject.Find("Camera");
        }
        positionDepart = cameraGO.transform.position;
        positionAlternatif = GameObject.Find("Point_Alt").transform.position;
        positionTan = GameObject.Find("Point_Tan").transform.position;
        if (GameObject.Find("Point_Alt") == null)
        {
            Debug.LogError("Sonde de position alternative manquante, traveling indisponible");
        }
        
    }
    public void Update()
    {
        if (GameManager.instance.replayer.GetButtonDown("Camera_Travel"))
        {
            travel = true;
            
        }
        if (travel)
        {
            Travel();
        }
    }
    public void Travel()
    {
        

        if (progress < 1.0f)
        {
            switch (position)
            {
                case false:
                {
                    progress += (Time.deltaTime * slowFactor);
                        vectorNormal = Vector3.Lerp(positionDepart, positionTan, progress);
                        vectorTan = Vector3.Lerp(positionTan, positionAlternatif, progress);
                        cameraGO.transform.position = Vector3.Lerp(vectorNormal, vectorTan, progress); 
                }
                break;
                case true:
                {
                    progress += (Time.deltaTime * slowFactor);
                        vectorNormal = Vector3.Lerp(positionAlternatif, positionTan, progress);
                        vectorTan = Vector3.Lerp(positionTan, positionDepart, progress);
                        cameraGO.transform.position = Vector3.Lerp(vectorNormal, vectorTan, progress);
                }
                break;
            }
        }
        else if (progress >= 1.0f)
        {
            progress = 0.0f;
            travel = false;
            position = !position;
        }
    }
}

