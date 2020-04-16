using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance = null;
    bool travel;
    public bool position;
    float progress = 0.0f;
    public GameObject cameraGO = GameObject.Find("Camera");
    public Vector3 positionDepart = GameObject.Find("Camera").transform.position;
    public Vector3 positionAlternatif = GameObject.Find("Point_Alt").transform.position;
    public Vector3 TanDepart = GameObject.Find("Point_TanOrg").transform.position;
    public Vector3 TanAlternatif = GameObject.Find("Point_TanAlt").transform.position;
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
        if (progress < cutoff)
        {
            switch (position)
            {
                case true:
                {
                    progress += (Time.deltaTime / slowFactor);
                    cameraGO.transform.position = Vector3.Lerp(cameraGO.transform.position, positionDepart, progress);
                    
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
                
                cameraGO.transform.position = positionDepart;
                position = false;
            }
            travel = false;
            progress = 0f;            
        }
    }
}

