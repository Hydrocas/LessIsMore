using Com.IsartDigital.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Sphere")]
    public int sphereCount = 10;
    public float spawnSphereFrequency = 1;
    public int spawnSphereNumber = 2;
    public GameObject spherePrefab = null;

    [Header("Cube")]
    public int cubeCount = 10;
    public float spawnCubeFrequency = 1;
    public int spawnCubeNumber = 3;
    public GameObject cubePrefab = null;

    [Space]
    public Transform spawnPoint = null;
    public Transform spawnPointRadius = null;
    public ColliderChild reSpawnArea = null;

    private Vector3 radiusSpawn;

    private void Start()
    {
        radiusSpawn = spawnPointRadius.position - spawnPoint.position;

        StartCoroutine(SpawnLevelObjectRoutine(cubeCount, spawnCubeNumber, spawnCubeFrequency, cubePrefab)); // Cube spawn
        StartCoroutine(SpawnLevelObjectRoutine(sphereCount, spawnSphereNumber, spawnSphereFrequency, spherePrefab)); // Sphere spawn

        reSpawnArea.OnCollisionTrigger += ReSpawnArea_OnCollisionTrigger;
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }

    private void ReSpawnArea_OnCollisionTrigger(Collider other)
    {
        SpawnLevelObject(other.gameObject);
    }

    IEnumerator SpawnLevelObjectRoutine(int spawnTotalNumber, int spawnNumber, float spawnFrequency, GameObject spawnedPrefab)
    {
        int numToSpawn = spawnNumber;

        while (true)
        {
            spawnTotalNumber -= spawnNumber;

            if (spawnTotalNumber < 0)
            {
                numToSpawn = spawnNumber + spawnTotalNumber;
            }

            for (int i = numToSpawn - 1; i >= 0; i--)
            {
                SpawnLevelObject(Instantiate(spawnedPrefab));
            }

            if (spawnTotalNumber < 0) break;

            yield return new WaitForSeconds(spawnFrequency);
        }

        Debug.Log("all " + spawnedPrefab + " spawned");
    }

    void SpawnLevelObject(GameObject levelObject)
    {
        Vector3 radiusPos = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * radiusSpawn;
        levelObject.transform.position = spawnPoint.position + radiusPos; // random position
        levelObject.transform.rotation = Quaternion.AngleAxis(Random.value * 360, Vector3.right); // random rotation
    }

    void TestVictory(int player1CubeGrabed, int player2CubeGrabed)
    {
        if (player1CubeGrabed + player2CubeGrabed < cubeCount) return;
        if (player1CubeGrabed + player2CubeGrabed > cubeCount)
        {
            Debug.LogWarning("Oh shit ! Les joueur on plus de cube que spawné");
            return;
        } 

        Time.timeScale = 0;
        Debug.Log("Win");
    }
}
