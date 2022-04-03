using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] private Attack boss;
    [SerializeField] private float damagePerParticle = 5f;
    private List<ParticleSystem.Particle> enteredParticles;
    private ParticleSystem ps;
  
    private void Start()
    {
        enteredParticles = new List<ParticleSystem.Particle>();
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()  
    {
        enteredParticles.Clear();
        //Get all particles that entered a box collider
        int numEntered = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles, out var enterData);
        
        for (int i = 0; i < numEntered; i++)
        {
            if (enterData.GetCollider(i, 0).gameObject == boss.gameObject)
            {
                boss.DamageDealt(damagePerParticle);
            }
        }
    }
}
