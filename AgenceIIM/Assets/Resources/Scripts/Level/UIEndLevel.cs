using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEndLevel : MonoBehaviour
{
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public Text textStar1;
    public Text textStar2;
    public Text textStar2End;
    public Text textStar3;
    public Text textStar3End;

    public Animator animStar1;
    public Animator animStar2;
    public Animator animStar3;

    public void OnClickLoadNextLevel()
    {
        GameManager.instance.LoadNextScene();
    }

    public void LoadStars(int nbMove, int minPoints2Star, int minPoints3Star)
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        StartCoroutine(WaitASecound(nbMove, minPoints2Star, minPoints3Star));
    }

    public IEnumerator WaitASecound(int nbMove, int minPoints2Star, int minPoints3Star)
    {
        star1.SetActive(true);
        animStar1.Play("Star1");
        yield return new WaitForSeconds(1f);
        star2.SetActive(nbMove <= minPoints2Star ? true : false);
        animStar2.Play("Star2");
        yield return new WaitForSeconds(1f);
        star3.SetActive(nbMove <= minPoints3Star ? true : false);
        animStar3.Play("Star3");
    }

    public void OnClickRestartLevel()
    {
        switch (GameManager.instance.idMonde)
        {
            case Monde.Monde1:
                SceneManager.LoadScene("1-" + GameManager.instance.idLevel.ToString());
                break;
            case Monde.Monde2:
                SceneManager.LoadScene("2-" + GameManager.instance.idLevel.ToString());
                break;
            case Monde.Monde3:
                SceneManager.LoadScene("3-" + GameManager.instance.idLevel.ToString());
                break;
            default:
                break;
        }

        //GameManager.instance.ResetParty();
        gameObject.SetActive(false);
    }

    public void OnClickBackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
