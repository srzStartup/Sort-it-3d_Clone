using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private ParticleEventSystem _particleEventChannel;
    [SerializeField] private List<ParticleSystem> _congratsParticles;

    public override void Awake()
    {
        base.Awake();

        _particleEventChannel.CongratsParticlePlayRequested += OnCongratsParticlePlayRequested;
        _particleEventChannel.CongratsParticleStopRequested += OnCongratsParticleStopRequested;
    }

    private void OnDestroy()
    {
        _particleEventChannel.CongratsParticlePlayRequested -= OnCongratsParticlePlayRequested;
        _particleEventChannel.CongratsParticleStopRequested -= OnCongratsParticleStopRequested;
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
