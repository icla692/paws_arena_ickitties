using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXByVelocity : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxSpeed = 15f;

    private ParticleSystem particles;
    private float maxEmission;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        maxEmission = particles.emission.rateOverTime.constant;
        particles.Play();
    }
    private void Update()
    {
        var particleEmission = particles.emission;
        particleEmission.rateOverTime = Mathf.Clamp((rb.velocity.magnitude / maxSpeed), 0, 1) * maxEmission;
    }

}
