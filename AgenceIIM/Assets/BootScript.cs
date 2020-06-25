using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BootScript : MonoBehaviour
{
    public GameObject menuPC;
    public GameObject menuMobile;

    void DeterminPlatform()
    {
        menuPC.SetActive(true);
        menuMobile.SetActive(true);
#if UNITY_EDITOR
        if (!(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android))
        {
            //Code Spécifique PC
            Destroy(menuMobile);
        }
        else
        {
            // Code Spécifique Mobile
            Destroy(menuPC);
        }
#else
        if (!(Application.platform == RuntimePlatform.Android))
        {
            //Code Spécifique PC
            Destroy(menuMobile);
        }
        else
        {
            //Code Spécifique Mobile 
            Destroy(menuPC);
        }
#endif
    }

    void Awake()
    {   
        menuPC = GameObject.Find("Scene_Menu_PC");
        menuMobile = GameObject.Find("Scene_Menu_Android");
        DeterminPlatform();
    }

    
}
