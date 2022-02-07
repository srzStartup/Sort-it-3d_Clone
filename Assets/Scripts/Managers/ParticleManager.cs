using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private List<ParticleSystem> _congratsParticles;

    [SerializeField] private ParticleEventSystem _particleEvent_Channel;

    public override void Awake()
    {
        base.Awake();

        _particleEvent_Channel.CongratsParticlePlayRequested += OnCongratsParticlePlayRequested;
        _particleEvent_Channel.CongratsParticleStopRequested += OnCongratsParticleStopRequested;
    }

    private void OnCongratsParticleStopRequested()
    {
        _congratsParticles.ForEach((particle) =>
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        });
    }

    private void OnCongratsParticlePlayRequested()
    {
        _congratsParticles.ForEach((particle) =>
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        });
    }
}
