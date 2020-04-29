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
    [Range(0, 100)] 
    public int slowFactor;
    [Range(0.001f, 0.99f)] 
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
        if(cameraGO == null || cameraGO.GetComponent<Camera>() == null) { 
            cameraGO = GetComponent<Camera>().gameObject;
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
        if (travel)
        {
            Travel();
        }
        else if (GameManager.instance.replayer.GetButtonDown("Camera_Travel"))
        {
            GameManager.instance.DATAnbMoveCam++;
            StartTravel();
        }
    }

    public void StartTravel()
    {
        if (travel) return;
        progress = 0.0f;
        travel = true;
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

