using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound = null;
    [SerializeField] private GameObject collisionParticle = null;
    [SerializeField] private string collisionTag = "Ground";
    [Space]
    public Vector2 pitchVariance = new Vector2(0.9f, 1.1f);

    private GameObject particle = null;

    private AudioSource audiosource = null;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(collisionTag))
        {
            SpawnParticleCollision(collision.gameObject, collision.GetContact(0).point);
            audiosource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
            audiosource.PlayOneShot(hitSound);
        }
    }

    private void SpawnParticleCollision(GameObject other, Vector3 contactPosition)
    {
        //Vector3 lookDirection = Vector3.Cross(contactPosition - transform.position, transform.right); OLD
        Vector3 lookDirection = transform.position - contactPosition;
        //lookDirection.y = 0;

        particle = Instantiate(collisionParticle);
        particle.transform.position = contactPosition;
        particle.transform.rotation = Quaternion.LookRotation(lookDirection);
        //particle.transform.rotation = Quaternion.AngleAxis(-90, particle.transform.forward) * particle.transform.rotation;

        Destroy(particle, 0.6f);
    }
}
