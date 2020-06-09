using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance = null;
    bool travel;
    [System.NonSerialized]
    public bool position;
    float progress = 0.0f;
    public GameObject mainCameraGO;
    public GameObject pipCameraGO;
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
        positionDepart = mainCameraGO.transform.position;
        positionAlternatif = GameObject.Find("Point_Alt").transform.position;
        positionTan = GameObject.Find("Point_Tan").transform.position;
        if (GameObject.Find("Point_Alt") == null)
        {
            Debug.LogError("Sonde de position alternative manquante, traveling indisponible");
        }
        mainCameraGO.transform.position = positionDepart;
        pipCameraGO.transform.position = positionAlternatif;
        
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
        GameObject crossUI = GameObject.Find("ControlIcon");
        if (position)
        {
            crossUI.GetComponent<RectTransform>().localRotation = new Quaternion(90, 52, -128, 0);
        }
        else
        {
            crossUI.GetComponent<RectTransform>().localRotation = new Quaternion(90, -38, -128, 0);
        }
        if (progress < 1.0f)
        {
            switch (position)
            {
                case false:
                {
                        progress += (Time.deltaTime * slowFactor);
                        vectorNormal = Vector3.Lerp(positionDepart, positionTan, progress);
                        vectorTan = Vector3.Lerp(positionTan, positionAlternatif, progress);
                        mainCameraGO.transform.position = Vector3.Lerp(vectorNormal, vectorTan, progress); 
                        pipCameraGO.transform.position = Vector3.Lerp(vectorTan, vectorNormal, progress); 
                }
                break;
                case true:
                {
                        progress += (Time.deltaTime * slowFactor);
                        vectorNormal = Vector3.Lerp(positionAlternatif, positionTan, progress);
                        vectorTan = Vector3.Lerp(positionTan, positionDepart, progress);
                        mainCameraGO.transform.position = Vector3.Lerp(vectorNormal, vectorTan, progress);
                        pipCameraGO.transform.position = Vector3.Lerp(vectorNormal, vectorTan, progress); 
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

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCameraGO.transform.position;
        Vector3 originalRot = mainCameraGO.transform.eulerAngles;

        float elapsed = 0.0f;

        mainCameraGO.GetComponent<LookAtConstraint>().enabled = false;
        while (elapsed < duration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;

            mainCameraGO.transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        mainCameraGO.transform.position = originalPos;
        mainCameraGO.transform.eulerAngles = originalRot;
        mainCameraGO.GetComponent<LookAtConstraint>().enabled = true;
        
    }
}

