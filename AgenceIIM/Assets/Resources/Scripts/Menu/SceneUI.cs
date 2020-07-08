using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    [SerializeField]private Text idLevelTrad;
    [SerializeField] private Text idLevelEnd;

    [SerializeField] private Text idLevelTradMobile;
    [SerializeField] private Text idLevelEndMobile;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            idLevelTrad = idLevelTradMobile;
            idLevelEnd = idLevelEndMobile;
        }

        if (idLevelTrad == null)
        idLevelTrad = GameObject.Find("levelIdTrad").GetComponent<Text>();
        if (idLevelEnd == null)
            idLevelEnd = GameObject.Find("LevelIdEnd").GetComponent<Text>();
    }

    void Update()
    {
        int i = 0;
        switch (GameManager.instance.idMonde)
        {
            case Monde.Monde1:
                i = 1;
                break;
            case Monde.Monde2:
                i = 2;
                break;
            case Monde.Monde3:
                i = 3;
                break;
        }

        idLevelEnd.text = string.Format(idLevelTrad.text, i, (GameManager.instance.idLevel+1).ToString());
    }
}
