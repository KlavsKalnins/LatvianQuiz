using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    public static ConfettiManager Instance;
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayParticle()
    {
        particleSystem.Play();
    }

}
