using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStatic : Cube
{
   public void ActivateStain(Color tint)
    {/*
        if (stain == null) return;
        if (colorPotencial == 0 && !isEnemy)
        {
            stain.SetActive(true);
            stain.GetComponent<Renderer>().material.color = tint;

            stain.transform.localScale = stainScale;
            elapsedTime = 0;

            StopCoroutine(StainRemove());
            StartCoroutine(StainRemove());
        }*/
    }

    private IEnumerator StainRemove()
    {/*
        Color colorFade = stain.GetComponent<Renderer>().material.color;
        Vector3 sizeShrink = stainScale;

        while (stain.GetComponent<Renderer>().material.color.a > 0)
        {           
            elapsedTime += Time.deltaTime;

            colorFade.a = fadeCurve.Evaluate(elapsedTime);
            sizeShrink.x = shrinkCurve.Evaluate(elapsedTime);
            sizeShrink.y = shrinkCurve.Evaluate(elapsedTime);

            stain.transform.localScale = sizeShrink;
            stain.GetComponent<Renderer>().material.color = colorFade;

            yield return null;
        }

        StainReset();
        */
        yield return null;
    }

    private void StainReset()
    {/*
        StopCoroutine(StainRemove());

        stain.GetComponent<Renderer>().material.color = stainColor;
        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        stain.SetActive(false);
        */
    }
    
}
