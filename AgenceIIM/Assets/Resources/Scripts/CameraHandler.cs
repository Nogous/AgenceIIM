using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance = null;
    bool travel;
    public bool position;
    float progress = 0.0f;
    public GameObject cameraGO;
    Vector3 positionDépart = GameObject.Find("Camera").transform.position;
    Vector3 positionAlternatif = GameObject.Find("Point_ALT").transform.position;
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
        positionDépart = cameraGO.transform.position;
        positionAlternatif = GameObject.Find("Point_ALT").transform.position;
        if (GameObject.Find("Point_ALT") == null)
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
                
                cameraGO.transform.position = positionAlternatif;
                position = true;
            }
            else
            {
                
                cameraGO.transform.position = positionDépart;
                position = false;
            }
            travel = false;
            progress = 0f;            
        }
    }
}

