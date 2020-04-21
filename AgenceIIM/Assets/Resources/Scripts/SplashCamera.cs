using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class SplashCamera : MonoBehaviour
{
    [SerializeField] private List<Sprite> stains = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateStains(Color paintColor)
    {
        int stainNum;

        stainNum = (int)Random.Range(3f, 8f);

        for(int i = 0; i < stainNum; i++)
        {
            GameObject stain = new GameObject();
            stain.AddComponent<SpriteRenderer>();
            SpriteRenderer renderer = stain.GetComponent<SpriteRenderer>();

            renderer.sprite = stains[(int)Random.value];
            renderer.color = paintColor;
        }
    }
}
