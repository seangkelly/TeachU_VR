using UnityEngine;
using System.Collections;

public class TriggerParticles : MonoBehaviour
{
    // Celebration!
    void OnTriggerEnter(Collider col)
    {
        GetComponent<ParticleSystem>().Stop();
        GetComponent<ParticleSystem>().Play();
    }
}
