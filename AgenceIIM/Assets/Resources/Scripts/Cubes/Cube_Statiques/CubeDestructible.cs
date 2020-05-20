using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestructible : CubeStatic
{
    [SerializeField] private ParticleSystem particle = null;
    
    public override void OnAwake()
    {
        base.OnAwake();

        cubeType = CubeType.Destructible;
    }
    
    public void Crumble()
    {
        ParticleSystem destroyParticle = Instantiate(particle, transform.position, Quaternion.identity);

        gameObject.SetActive(false);

        destroyParticle.Play();
    }
}
