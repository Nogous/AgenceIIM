﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEndLevel : MonoBehaviour
{
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public Text textStar1;
    public Text textStar2;
    public Text textStar3;

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