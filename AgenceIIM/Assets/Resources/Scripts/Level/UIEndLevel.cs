using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndLevel : MonoBehaviour
{
    public void OnClickLoadNextLevel()
    {
        GameManager.instance.LoadNextScene();
    }

    public void OnClickRestartLevel()
    {
        GameManager.instance.ResetParty();
        gameObject.SetActive(false);
    }

    public void OnClickBackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
