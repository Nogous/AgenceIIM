﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStatic : Cube
{
    [Header("Options des effets de taches")]

    [SerializeField] protected GameObject stain = null;
    protected Vector3 stainScale = Vector3.one;
    protected Color stainColor = Color.white;

    [SerializeField] private AnimationCurve fadeCurve = null;
    [SerializeField] private AnimationCurve shrinkCurve = null;

    private float elapsedTime = 0;

    protected bool isBreakable = true;

    public bool IsBreakable
    {
        get { return isBreakable; }
    }

    public void ActivateStain(Color tint)
    {
        if (stain == null) return;
        
        stain.SetActive(true);
        stain.GetComponent<Renderer>().material.color = tint;

        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        StopCoroutine(StainRemove());
        StartCoroutine(StainRemove());
    }

    private IEnumerator StainRemove()
    {
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
        
        yield return null;
    }

    private void StainReset()
    {
        StopCoroutine(StainRemove());

        stain.GetComponent<Renderer>().material.color = stainColor;
        stain.transform.localScale = stainScale;
        elapsedTime = 0;

        stain.SetActive(false);
        
    }
    
}
