using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClampPhysicsSpeed : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] private float maxSpeed = 10;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
    }
}
