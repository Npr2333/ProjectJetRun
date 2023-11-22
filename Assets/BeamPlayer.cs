using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPlayer : MonoBehaviour
{
    public ParticleSystem beam;
    
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem Lavabeam = Instantiate(beam, transform.position, transform.rotation);
        Lavabeam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
