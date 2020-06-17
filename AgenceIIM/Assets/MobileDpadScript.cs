using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDpadScript : MonoBehaviour
{
    public Player player;

    void Start()
    {
        player = GameManager.instance.player;
    }
    
    public void ProcessMobileInputLeft()
    {
        player.StartCoroutine(player.MobileLeftAxisBehaviour());
    }

    public void ProcessMobileInputRight()
    {
        player.StartCoroutine(player.MobileRightAxisBehaviour());
    }

    public void ProcessMobileInputDown()
    {
        player.StartCoroutine(player.MobileDownAxisBehaviour());
    }

    public void ProcessMobileInputUp()
    {
        player.StartCoroutine(player.MobileUpAxisBehaviour());
    }



}
