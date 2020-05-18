using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTNT : CubeStatic
{

    protected Vector3[] vectorsTNT = new Vector3[4] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    [Header("Options de l'explosion")]

    public float explosionForce = 50f;
    public float explosionRayon = 4f;
    public float explosionUpward = 0.04f;

    [SerializeField] private float tntDelay = 1f;
    [SerializeField] private ParticleSystem particleTnt = null;

    public override void Awake()
    {
        cubeType = CubeType.TNT;
    }


    public void Explode(bool isPlayer = false)
    {
        AudioManager.instance.Play("TNT");
        StartCoroutine(DetonateTnt());
    }

    private IEnumerator DetonateTnt()
    {
        yield return new WaitForSeconds(tntDelay);

        DestroySurroundings();

        ParticleSystem tntExplosion = Instantiate(particleTnt, transform.position, Quaternion.identity);
        tntExplosion.Play();

        gameObject.SetActive(false);
    }

    private void DestroySurroundings()
    {
        bool isPlayerDestroyed = false;

        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(transform.position, vectorsTNT[i], out hit, 1f))
            {

                if (hit.transform.gameObject.GetComponent<Cube>())
                {
                    Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                    //tmpCube.Explode();
                }
                else if (hit.transform.parent != null)
                {
                    if (hit.transform.parent.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                    {
                        hit.transform.parent.parent.gameObject.GetComponent<Player>().SetDeath();
                        isPlayerDestroyed = true;
                    }
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[0] + vectorsTNT[2], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                //tmpCube.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }

        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[0] + vectorsTNT[3], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                //tmpCube.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[1] + vectorsTNT[2], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                //tmpCube.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[1] + vectorsTNT[3], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                //tmpCube.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }

        //SetModeVoid();
    }

}
