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
        positionAlternatif = pipCameraGO.transform.position;
        positionTan = GameObject.Find("Point_Tan").transform.position;
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
        var positionTargetMain = pipCameraGO.transform.position;
        var positionTargetPip = mainCameraGO.transform.position;
        pipCameraGO.transform.position = positionTargetPip;
        mainCameraGO.transform.position = positionTargetMain;
        travel = false;
        position = !position;
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

