using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaytestAnalitic : MonoBehaviour
{
    public static PlaytestAnalitic Instance = null;

    public Text data;

    public int nbLevelToTest = 1;

    [HideInInspector]public int[] nbDeath;
    [HideInInspector] public float[] timeDuration;
    [HideInInspector] public int[] nbMoveCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        data.gameObject.SetActive(false);

        nbDeath = new int[nbLevelToTest];
        timeDuration = new float[nbLevelToTest];
        nbMoveCam = new int[nbLevelToTest];
}

    public void ShowData(int idLevel)
    {
        if (idLevel < nbLevelToTest-1)
        {
            return;
        }

        data.gameObject.SetActive(true);
        data.text = "";

        float totalMort = 0;
        for (int i = nbDeath.Length; i-- > 0;)
        {
            totalMort += nbDeath[i];
        }

        data.text += "NombreDeMort : " + totalMort + "\n";

        Debug.Log(nbDeath.Length);
        for (int i = nbDeath.Length; i-- > 0;)
        {
            if (timeDuration[i] <= 0)
            {
                break;
            }
            data.text += "Mort level " + i + " : " + nbDeath[i] + "\n";
        }

        totalMort = 0;

        for (int i = timeDuration.Length; i-- > 0;)
        {
            totalMort += timeDuration[i];
        }

        data.text += "Temps de jeu : " + totalMort + "\n";

        for (int i = nbDeath.Length; i-- > 0;)
        {
            if (timeDuration[i] <= 0)
            {
                break;
            }
            data.text += "Temps level " + i + " : " + timeDuration[i] + "\n";
        }

        totalMort = 0;

        for (int i = nbMoveCam.Length; i-- > 0;)
        {
            totalMort += nbMoveCam[i];
        }

        data.text += "Nombre de rotation de la camera : " + totalMort + "\n";

        for (int i = nbMoveCam.Length; i-- > 0;)
        {
            if (timeDuration[i] <= 0)
            {
                break;
            }
            data.text += "Nombre de rotation de la camera level " + i + " : " + nbMoveCam[i] + "\n";
        }
    }

    public void HideData()
    {
        data.gameObject.SetActive(false);
    }
}
