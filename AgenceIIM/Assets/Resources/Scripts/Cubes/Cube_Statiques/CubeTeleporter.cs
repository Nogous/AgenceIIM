using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTeleporter : CubeStatic
{
    [SerializeField] private Cube teleportDestination = null;

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
        player.gameObject.transform.position = new Vector3(teleportDestination.transform.position.x, teleportDestination.transform.position.y + 1f, teleportDestination.transform.position.z);
        /*Ligne de code pour le son de teleport
        AudioManager.instance.Play(pouet); 
        */
    }
}
