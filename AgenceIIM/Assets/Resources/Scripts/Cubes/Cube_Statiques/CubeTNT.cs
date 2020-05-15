using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTNT : CubeStatic
{
    
    
    [Header("Options de l'explosion")]

    public float explosionForce = 50f;
    public float explosionRayon = 4f;
    public float explosionUpward = 0.04f;

    [SerializeField] private float tntDelay = 1f;
    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private int cubesInRow = 5;
    [SerializeField] private ParticleSystem particleDeath = null;
    [SerializeField] private ParticleSystem particleTnt = null;
    private Cube associatedTnt = null;
    void Awake()
    {
        cubeType = CubeType.TNT;
    }

    
    public void Explode(bool isPlayer = false)
    {
        if (isTnt)
        {
            AudioManager.instance.Play("TNT");
            StartCoroutine(DetonateTnt());
            return;
        }

        if (!isPlayer)
        {
            if (!isBreakable) return;

            if (isEnemyMirror || isEnemyMoving) Player.OnMove -= SetModeMove;

            if (isEnemy)
            {
                GameManager.instance.KillEnnemy();

                ParticleSystem particles = Instantiate(particleDeath, transform.position, Quaternion.identity);

                ParticleSystem.MainModule mainMod = particles.main;

                mainMod.startColor = color;

                particles.Play();
            }
            //make object disappear
            gameObject.SetActive(false);
        }
        else
        {
            AudioManager.instance.Play("Splash");
            AudioManager.instance.Play("ExplosionCube");
        }

        // loop 3 times to create 5x5x5 pices un x,y,z coordonate
        /*for (int i = cubesInRow; i-->0;)
        {
            for (int j = cubesInRow; j-- > 0;)
            {
                for (int k = cubesInRow; k-- > 0;)
                {
                    CreatePiece(i, j, k);
                }
            }
        }*/

        // get explosion position
        Vector3 explosionPos = transform.position;
        // get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRayon);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // addd explosion force
                rb.AddExplosionForce(explosionForce, transform.position, explosionRayon, explosionUpward);
            }
        }
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
            if (Physics.Raycast(transform.position, vectors[i], out hit, 1f))
            {

                if (hit.transform.gameObject.GetComponent<Cube>())
                {
                    Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                    tmpCube.Explode();
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

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectors[0] + vectors[2], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                tmpCube.Explode();
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

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectors[0] + vectors[3], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                tmpCube.Explode();
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

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectors[1] + vectors[2], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                tmpCube.Explode();
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

        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0), vectors[1] + vectors[3], out hit, 1f))
        {
            if (hit.transform.gameObject.GetComponent<Cube>())
            {
                Cube tmpCube = hit.transform.gameObject.GetComponent<Cube>();

                tmpCube.Explode();
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

        SetModeVoid();
    }

}
