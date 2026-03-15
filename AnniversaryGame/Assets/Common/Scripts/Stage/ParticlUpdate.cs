using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlUpdate : MonoBehaviour
{

    [SerializeField] ParticleSystem m_particleSystem;

    public void StartEffect()
    {
        m_particleSystem?.Play();
    }

    public void StopEffect()
    {
        m_particleSystem?.Stop();
    }

    public void ClearEffect()
    {
        m_particleSystem?.Clear();
    }
}
