using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingLight
{
    public Light light;
    public ParticleSystem shootingParticle;
    public float lightIntensity;
    [HideInInspector] public float currentIntensity;
}
