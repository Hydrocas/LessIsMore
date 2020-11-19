using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound = null;
    [SerializeField] private GameObject collisionParticle = null;

    private GameObject particle = null;

    private AudioSource audiosource = null;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        audiosource.PlayOneShot(hitSound);
        /*
        particle = Instantiate(collisionParticle);
        particle.transform.position = transform.position;
        Destroy(particle,3f);
        */
    }
}
