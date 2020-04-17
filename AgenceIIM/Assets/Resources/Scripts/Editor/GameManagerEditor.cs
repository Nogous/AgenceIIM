using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    void OnEnable()
    {
        GameManager gameManager = (GameManager)target;

        if (GameManager.instance == null)
        {
            GameManager.instance = gameManager;
        }
    }
}
