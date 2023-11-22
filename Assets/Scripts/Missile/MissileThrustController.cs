using UnityEngine;

public class MissileThrustController: MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float repeatRate = 5.0f;

    private void Start()
    {
        InvokeRepeating("PlayParticle", 0.0f, repeatRate);
    }

    void PlayParticle()
    {
        particleSystem.Play();
    }
}

