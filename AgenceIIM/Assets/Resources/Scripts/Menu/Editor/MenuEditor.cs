using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Menu))]
[CanEditMultipleObjects]
public class MenuEditor : Editor
{
    Menu myMenu;

    void OnEnable()
    {
        myMenu = (Menu)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset Score"))
        {
            SaveSystem.SavePoints(new int[0], "starsMonde1");
            SaveSystem.SavePoints(new int[0], "starsMonde2");
            SaveSystem.SavePoints(new int[0], "starsMonde3");
        }

        if (GUILayout.Button("Unlock All"))
        {
            for (int i = 0; i < myMenu.levelUIMonde1.Length; i++)
            {
                if (myMenu.levelUIMonde1[i].button != null)
                {
                    myMenu.levelUIMonde1[i].button.interactable = true;
                }
            }
        }

        if (GUILayout.Button("SetQwerty"))
        {
        }
        if (GUILayout.Button("SetAzerty"))
        {
            GameManager.isQwerty = false;
            Debug.Log(GameManager.isQwerty);

            myMenu.managerAzerty.enabled = true;
            myMenu.managerQwerty.enabled = false;
        }
    }
}
