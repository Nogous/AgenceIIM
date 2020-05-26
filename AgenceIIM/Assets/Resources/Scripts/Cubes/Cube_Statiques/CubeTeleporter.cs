using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTeleporter : CubeStatic
{
    [SerializeField] private Cube teleportDestination = null;
    public float dislovationAnimationDuration;
    public AnimationCurve dislovationAnimationCurve;

    public override void OnAwake()
    {
        base.OnAwake();
        cubeType = CubeType.Teleporter;
    }

    public Cube GetDestination()
    {
        return teleportDestination;
    }

    public void TeleportPlayer(Player player){
        StartCoroutine(Teleporter(player)); 
    }

    private IEnumerator Teleporter(Player player){
        float timeElapsed = 0f;
        while(timeElapsed < dislovationAnimationDuration){
            timeElapsed+= Time.deltaTime;
            //*shader value* = dislovationAnimationCurve.Evaluate(timeElapsed);
        }
        player.gameObject.transform.position = new Vector3(teleportDestination.transform.position.x, teleportDestination.transform.position.y + 1f, teleportDestination.transform.position.z);
        /*Ligne de code pour le son de teleport
        AudioManager.instance.Play(pouet); 
        */
        while(timeElapsed > 0){
            timeElapsed-= Time.deltaTime;
            //*shader value* = dislovationAnimationCurve.Evaluate(timeElapsed);
        }
        yield return null;
    }


}
