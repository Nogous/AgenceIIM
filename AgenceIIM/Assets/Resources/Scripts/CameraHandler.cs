using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance = null;
    bool travel;
    public bool position;
    float progress = 0.0f;
    Vector3 positionDépart;
    [Header("Renseignez la caméra et sa position")]
    public GameObject cameraGO;
    public Vector3 positionAlternatif;
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
            Debug.LogError("Aucune Camera renseignée ! Veuillez utiliser de préférence la caméra présente de le dossier Ressources/Prefab/Camera");
        }
        positionDépart = cameraGO.transform.position;
        if (positionAlternatif == new Vector3(0, 0, 0) || positionAlternatif == null)
        {
            Debug.LogError("La position alternative ne peut pas être à l'origine de la scene");
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
                    position = false;
                }
                break;
                case false:
                {
                    progress += (Time.deltaTime / slowFactor);
                    cameraGO.transform.position = Vector3.Lerp(cameraGO.transform.position, positionAlternatif, progress);
                    position = true;
                }
                break;
            }
        }
        else
        {
            if (!position)
            {
                
                cameraGO.transform.position = positionAlternatif;
            }
            else
            {
                
                cameraGO.transform.position = positionDépart;
            }
            travel = false;
            progress = 0f;            
        }
    }
}

