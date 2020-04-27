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

            Vector3 collideRot = Vector3.zero;

            if (collisionEvents[i].normal == Vector3.forward)
            {
                collideRot = new Vector3(90, 0, 0);
            }
            else if (collisionEvents[i].normal == Vector3.back)
            {
                collideRot = new Vector3(-90, 0, 0);
            }
            else if (collisionEvents[i].normal == Vector3.left)
            {
                collideRot = new Vector3(0, 0, 90);
            }
            else if (collisionEvents[i].normal == Vector3.right)
            {
                collideRot = new Vector3(0, 0, -90);
            }

            GameObject newStain = Instantiate(stain, collidePos, Quaternion.Euler(collideRot));

            newStain.transform.localScale *= Random.Range(0.2f, 1.5f);

            newStain.GetComponent<MeshRenderer>().material.SetColor("_Color", main.startColor.color);
        }

    }
}
