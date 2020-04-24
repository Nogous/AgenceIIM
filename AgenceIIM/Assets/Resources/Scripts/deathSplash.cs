using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathSplash : MonoBehaviour
{
    private ParticleSystem particle;
    public List<ParticleCollisionEvent> collisionEvents;

    [SerializeField] private GameObject stain;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);

        ParticleSystem.MainModule main = particle.main;

        for (int i = 0; i < numCollisionEvents; i++)
        {

            Vector3 collidePos = collisionEvents[i].intersection;

            GameObject newStain = Instantiate(stain, collidePos, Quaternion.identity);

            newStain.GetComponent<MeshRenderer>().material.SetColor("_Color", main.startColor.color);
        }

    }
}
