using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnHoverBnt(string Bool)
    {
        if(animator != null)
        {
            if (!animator.GetBool(Bool)) animator.SetBool(Bool, true);
        }
    }
    public void OnHoverExit(string Bool)
    {
        if (animator != null)
        {
            if (animator.GetBool(Bool)) animator.SetBool(Bool, false);
        }
    }

    public void OnEndAnimToOptions()
    { 
        FindObjectOfType<Menu>().OnClickOption();
    }

    public void OnEndAnimToLevels()
    { 
        FindObjectOfType<Menu>().OnClickSelectWorldMenu();
    }

    public void OnEndAnimToMain()
    {
        FindObjectOfType<Menu>().OnClickMainMenu();
    }
}
