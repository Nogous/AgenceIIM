using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFade : MonoBehaviour
{
    [SerializeField] private float timeToFade = 5;

    [SerializeField] private AnimationCurve fadeAnim = null;

    private float elapsedTime = 0;
    private float curveTime = 0;

    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;     
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= timeToFade)
        {
            curveTime += Time.deltaTime;

            startScale *= fadeAnim.Evaluate(curveTime);

            transform.localScale = startScale;

            if (curveTime >= fadeAnim.length) Destroy(gameObject);
        }
    }
}
