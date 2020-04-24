using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileButtonHandler : MonoBehaviour
{
   
    public void OnClick_Reset()
    {
        GameManager.instance.ResetParty();
    }

    public void OnClick_Travel()
    {
        CameraHandler.instance.StartTravel();
    }
}
