using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Particle Event Channel")]
public class ParticleEventSystem : ScriptableObject
{
    public UnityAction CongratsParticlePlayRequested;
    public UnityAction CongratsParticleStopRequested;

    public void RaiseCongratsParticlePlayRequestEvent()
    {
        CongratsParticlePlayRequested?.Invoke();
    }

    public void RaiseCongratsParticleStopRequestEvent()
    {
        CongratsParticleStopRequested?.Invoke();
    }
}
