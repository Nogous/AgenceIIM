using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashCamera : MonoBehaviour
{
    public static SplashCamera instance = null;

    [SerializeField] private List<Image> stains = new List<Image>();

    private List<Image> addedStains = new List<Image>();

    private RectTransform rectangle;

    [SerializeField] private AnimationCurve ScaleCurve = null;
    [SerializeField] private AnimationCurve FadeCurve = null;

    private float elapsedTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rectangle = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(addedStains.Count > 0)
        {
            elapsedTime += Time.deltaTime;

            for(int i = addedStains.Count - 1; i >= 0; i--)
            {
                Color fadeColor = addedStains[i].color;

                addedStains[i].transform.localScale *= ScaleCurve.Evaluate(elapsedTime);
                fadeColor.a *= FadeCurve.Evaluate(elapsedTime);
                addedStains[i].color = fadeColor;
            }

            if(elapsedTime >= ScaleCurve.keys[2].time)
            {
                elapsedTime = 0;
            }
        }
    }

    public void CreateStains(Color paintColor)
    {
        int stainNum;

        stainNum = (int)Random.Range(5f, 12f);

        for(int i = 0; i < stainNum; i++)
        {
            Image newStain;

            Image stain = stains[(int)(stains.Count * Random.value)];
            Vector3 stainPos = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 5);

            Vector3 stainLocalPos = Camera.main.ScreenToWorldPoint(stainPos);

            newStain = Instantiate(stain, stainLocalPos, transform.rotation, transform);
            addedStains.Add(newStain);

            newStain.color = paintColor;

            newStain.transform.localScale *= Random.Range(2, 5);
        }
    }
}
