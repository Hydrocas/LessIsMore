using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Transform camera1Pos;
    [Space] 
    [SerializeField] private Camera camera2;
    [SerializeField] private Transform camera2Pos;
    [Space]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject splitScreen;
    [SerializeField] private float distanceToSplit = 1000;

    private void Awake()
    {
        splitScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.LookAt(Vector3.up * 3);
        Vector3 targetPosition = Vector3.Slerp(camera1Pos.position, camera2Pos.position, 0.5f);
        targetPosition.y = camera1Pos.position.y;
        mainCamera.transform.position = targetPosition;

        //Debug.Log((camera1Pos.position - camera2Pos.position).magnitude);
        bool isAboveDistanceToSplit = (camera1Pos.position - camera2Pos.position).magnitude > distanceToSplit;

        if (isAboveDistanceToSplit && !splitScreen.activeSelf)
        {
            splitScreen.SetActive(true);
        }
        else if (!isAboveDistanceToSplit && splitScreen.activeSelf)
        {
            splitScreen.SetActive(false);
        }
    }
}
