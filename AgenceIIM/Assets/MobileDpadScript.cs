using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDpadScript : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    public void ProcessMobileInputLeft()
    {
        StartCoroutine(player.MobileLeftAxisBehaviour());
    }

    public void ProcessMobileInputRight()
    {
        StartCoroutine(player.MobileRightAxisBehaviour());
    }

    public void ProcessMobileInputDown()
    {
        StartCoroutine(player.MobileDownAxisBehaviour());
    }

    public void ProcessMobileInputUp()
    {
        StartCoroutine(player.MobileUpAxisBehaviour());
    }



}
