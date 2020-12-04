using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StretchToPhysicsVelocity : MonoBehaviour
{
    [SerializeField] private float targetSpeed = 20;
    [SerializeField] private float targetStrechPower = 1.3f;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 physicsVelocity = rigidbody.velocity;
        float strechPower = Mathf.Lerp(1, targetStrechPower, physicsVelocity.magnitude / targetSpeed);
        transform.localScale = physicsVelocity.normalized * strechPower;
    }
}
