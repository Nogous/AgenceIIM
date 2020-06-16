using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnHomeScreen : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameManager.instance.GoToMainMenu();
    }
}
