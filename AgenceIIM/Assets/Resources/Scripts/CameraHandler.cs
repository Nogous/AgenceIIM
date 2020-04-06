using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    bool travel;
    float progress = 0.0f;
    private GameObject camera;

    public void TravelTick()
    {
        travel = true;
    }

    public void Start()
    {
        camera = GameObject.Find("Camera");
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
        if(progress < 1.0f)
        {
            progress += (Time.deltaTime / 50);
            Vector3 positionTarget = new Vector3(4.9f, 9.9f, 4.9f);
            camera.transform.position = Vector3.Lerp(camera.transform.position, positionTarget, progress);
        }
        else
        {
            travel = false;
        }
    }
}

