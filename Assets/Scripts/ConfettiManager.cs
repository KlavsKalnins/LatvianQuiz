using Sirenix.OdinInspector;
using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    public static ConfettiManager Instance;
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        Instance = this;
    }
    
    [Button]
    public void PlayParticle()
    {
        particleSystem.Play();
    }

}
