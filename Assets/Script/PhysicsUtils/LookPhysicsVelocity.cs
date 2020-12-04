using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LookPhysicsVelocity : MonoBehaviour
{
    [SerializeField] private new Transform transform = null;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 lookDirection = rigidbody.velocity.normalized;

        if (lookDirection == Vector3.zero)
            return;

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
