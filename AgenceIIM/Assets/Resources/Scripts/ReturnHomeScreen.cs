using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnHomeScreen : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameManager.instance.GoToMainMenu();
    }

    public void CameraTravel()
    {
        CameraHandler.instance.StartTravel();
    }

    public void ResetLevel()
    {
        GameManager.instance.ResetParty();
    }
}
