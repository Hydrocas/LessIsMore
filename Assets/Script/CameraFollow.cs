using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target = null;
    public float easeRatio = 0.1f;

    void Start()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, easeRatio * Time.deltaTime); // rotation
        transform.position = Vector3.Lerp(transform.position, target.position, easeRatio * Time.deltaTime); // position
    }
}
