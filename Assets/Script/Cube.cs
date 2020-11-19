using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound = null;

    private AudioSource audiosource = null;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        audiosource.PlayOneShot(hitSound);
    }
}
