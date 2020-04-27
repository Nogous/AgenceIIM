using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyTester : MonoBehaviour
{
    public float force = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Transform selection = hit.transform;
                Jellyfier selectionJellyfier = selection.GetComponent<Jellyfier>();
                if (selectionJellyfier != null)
                {
                    selectionJellyfier.ApplyPressureToPoint(selection.position, force);
                    GameObject obj = Instantiate(new GameObject(selection.position.ToString()));
                    obj.transform.position = selection.position;
                }
            }
        }
    }
}
