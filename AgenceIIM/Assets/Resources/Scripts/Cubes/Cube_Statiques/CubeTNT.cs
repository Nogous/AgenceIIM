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

    public bool asExplod = false;
    public bool asReset = false;

    [SerializeField] private float tntDelay = 1f;
    [SerializeField] private ParticleSystem particleTnt = null;

    private ParticleSystem tntExplosion;

    public override void OnAwake()
    {
        base.OnAwake();
        
        cubeType = CubeType.TNT;
    }


    public override void ResetCube()
    {
        base.ResetCube();
        
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(true);
        asExplod = false;
        asReset = true;
        AudioManager.instance.Stop("TNT");
    }

    public void Explode(bool isPlayer = false)
    {
        if (asExplod) return;
        asExplod = true;
        AudioManager.instance.Play("TNT");
        StartCoroutine(DetonateTnt());
        asReset = false;
        if (tntExplosion != false)
        {
            Destroy(tntExplosion.gameObject);
        }
    }

    private IEnumerator DetonateTnt()
    {
        yield return new WaitForSeconds(tntDelay);
        if (!asReset)
        {

            DestroySurroundings();

            tntExplosion = Instantiate(particleTnt, transform.position, Quaternion.identity);
            tntExplosion.Play();
            Destroy(tntExplosion.gameObject, 10f);

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(6.0f);
            if (!asReset)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void DestroySurroundings()
    {
        bool isPlayerDestroyed = false;

        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(transform.position, vectorsTNT[i], out hit, 1f))
            {
                if (hit.transform.GetComponent<CubeTNT>())
                {
                    CubeTNT tnt = hit.transform.GetComponent<CubeTNT>();

                    tnt.DetonateTnt();
                }
                else if (hit.transform.gameObject.GetComponent<CubeStatic>())
                {
                    CubeStatic tmpCube = hit.transform.gameObject.GetComponent<CubeStatic>();

                    if(tmpCube.IsBreakable) tmpCube.gameObject.SetActive(false);
                }
                else if (hit.transform.gameObject.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                    enemy.Explode();
                }
                else if (hit.transform.parent != null)
                {
                    if (hit.transform.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                    {
                        hit.transform.parent.gameObject.GetComponent<Player>().SetDeath();
                        isPlayerDestroyed = true;
                    }
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[0] + vectorsTNT[2], out hit, 1f))
        {
            if (hit.transform.GetComponent<CubeTNT>())
            {
                CubeTNT tnt = hit.transform.GetComponent<CubeTNT>();

                tnt.DetonateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                CubeStatic tmpCube = hit.transform.gameObject.GetComponent<CubeStatic>();

                if (tmpCube.IsBreakable) tmpCube.gameObject.SetActive(false);
            }
            else if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                enemy.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }

        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[0] + vectorsTNT[3], out hit, 1f))
        {
            if (hit.transform.GetComponent<CubeTNT>())
            {
                CubeTNT tnt = hit.transform.GetComponent<CubeTNT>();

                tnt.DetonateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                CubeStatic tmpCube = hit.transform.gameObject.GetComponent<CubeStatic>();

                if (tmpCube.IsBreakable) {
                    tmpCube.gameObject.SetActive(false);
                } 
            }
            else if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                Debug.Log("kill ennemy");
                enemy.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[1] + vectorsTNT[2], out hit, 1f))
        {
            if (hit.transform.GetComponent<CubeTNT>())
            {
                CubeTNT tnt = hit.transform.GetComponent<CubeTNT>();

                tnt.DetonateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                CubeStatic tmpCube = hit.transform.gameObject.GetComponent<CubeStatic>();

                if (tmpCube.IsBreakable) tmpCube.gameObject.SetActive(false);
            }
            else if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                enemy.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectorsTNT[1] + vectorsTNT[3], out hit, 1f))
        {
            if (hit.transform.GetComponent<CubeTNT>())
            {
                CubeTNT tnt = hit.transform.GetComponent<CubeTNT>();

                tnt.DetonateTnt();
            }
            else if (hit.transform.gameObject.GetComponent<CubeStatic>())
            {
                CubeStatic tmpCube = hit.transform.gameObject.GetComponent<CubeStatic>();

                if (tmpCube.IsBreakable) tmpCube.gameObject.SetActive(false);
            }
            else if (hit.transform.gameObject.GetComponent<Enemy>())
            {
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();

                enemy.Explode();
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.gameObject.GetComponent<Player>() && !isPlayerDestroyed)
                {
                    hit.transform.parent.gameObject.GetComponent<Player>().SetDeath();
                    isPlayerDestroyed = true;
                }
            }
        }
    }

}
