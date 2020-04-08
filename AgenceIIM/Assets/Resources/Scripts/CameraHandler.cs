using System.Collections;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    bool travel;
    bool position;
    float progress = 0.0f;
    Vector3 positionDépart;
    [Header("Renseignez la caméra et sa position")]
    public GameObject cameraGO;
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
        try
        {
            cameraGO.name = "Camera";
        }
        catch(UnassignedReferenceException)
        {
            Debug.LogWarning("Aucune caméra n'a été renseignée, tentative de recherche de caméra ...");
            cameraGO = GameObject.Find("Camera");
            try
            {
                cameraGO.name = "Camera";
            }
            catch (MissingReferenceException)
            {
                cameraGO = GameObject.Find("Main Camera");
                try
                {
                    cameraGO.name = "Main Camera";
                }
                catch (MissingReferenceException)
                {
                    Debug.LogError("Tentative de réccupération échouée");
                }
            }
        }
        positionDépart = cameraGO.transform.position;
        if(positionDépart == new Vector3(0,0,0) || positionDépart == null)
        {
            Debug.LogError("Le script n'a pas trouvé de camera");
        }
        if (positionAlternatif == new Vector3(0, 0, 0) || positionAlternatif == null)
        {
            Debug.LogError("La position alternative ne peut pas être à l'origine de la scene");
        }
    }
    public void Update()
    {
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

