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

    public void TeleportPlayer(CubeMovable cube){
        StartCoroutine(Teleporter(cube)); 
    }

    private IEnumerator Teleporter(CubeMovable cube)
    {
        float timeElapsed = 0f;
        while(timeElapsed < dislovationAnimationDuration){
            timeElapsed+= Time.deltaTime;
            //*shader value* = dislovationAnimationCurve.Evaluate(timeElapsed);
        }
        cube.gameObject.transform.position = new Vector3(teleportDestination.transform.position.x, teleportDestination.transform.position.y + 1f, teleportDestination.transform.position.z);

        if (cube.gameObject.GetComponent<CubePush>())
        {
            if (!cube.gameObject.GetComponent<CubePush>().TestWall()) cube.SetModeMove(Vector3.zero);
            else
            {
                cube.orientation *= -1;
                cube.SetModeMove(Vector3.zero);
            }
        }
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
