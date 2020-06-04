using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : MonoBehaviour
{
    public Text idLevelTrad;
    public Text idLevelEnd;

    // Start is called before the first frame update
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

        idLevelEnd.text = string.Format(idLevelTrad.text, i, GameManager.instance.idLevel);
    }
}
