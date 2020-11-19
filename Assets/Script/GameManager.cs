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

    public Transform spawnPoint = null;
    public Transform spawnPointRadius = null;


    private float deltaTime = 0;
    private Vector3 radiusSpawn;

    private int totalCubeCount;

    private void Start()
    {
        radiusSpawn = spawnPointRadius.position - spawnPoint.position;
        totalCubeCount = cubeCount;

        StartCoroutine(SpawnLevelObjectRoutine(cubeCount, spawnCubeNumber, spawnCubeFrequency, cubePrefab)); // Cube spawn
        StartCoroutine(SpawnLevelObjectRoutine(sphereCount, spawnSphereNumber, spawnSphereFrequency, spherePrefab)); // Sphere spawn
    }

    #region Spawn Routine

    IEnumerator SpawnLevelObjectRoutine(int spawnTotalNumber, int spawnNumber, float spawnFrequency, GameObject spawnedPrefab)
    {
        //int spawnCount = spawnNumber;
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

    IEnumerator SpawnCubeRoutine()
    {
        while (true)
        {
            cubeCount -= spawnCubeNumber;

            int numCubeToSpawn = cubeCount < 0 ? spawnCubeNumber + cubeCount : spawnCubeNumber;

            for (int i = numCubeToSpawn - 1; i >= 0; i--)
            {
                SpawnCube(Instantiate(cubePrefab));
            }

            if (numCubeToSpawn < 0) break;

            yield return new WaitForSeconds(spawnCubeFrequency);
        }

        Debug.Log("all cube spawned");
    }

    IEnumerator SpawnSphereRoutine()
    {
        while (true)
        {
            sphereCount -= spawnCubeNumber;

            int numSphereToSpawn = sphereCount < 0 ? spawnSphereNumber + sphereCount : spawnSphereNumber;

            for (int i = numSphereToSpawn - 1; i >= 0; i--)
            {
                SpawnSphere(Instantiate(spherePrefab));
            }

            if (numSphereToSpawn < 0) break;

            yield return new WaitForSeconds(spawnSphereFrequency);
        }

        Debug.Log("all sphere spawned");
    }

    #endregion

    void SpawnSphere(GameObject cube)
    {
        SpawnLevelObject(cube);
    }

    void SpawnCube(GameObject cube)
    {
        SpawnLevelObject(cube);
    }

    void SpawnLevelObject(GameObject levelObject)
    {
        Vector3 radiusPos = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * radiusSpawn;
        levelObject.transform.position = spawnPoint.position + radiusPos; // random position
        levelObject.transform.rotation = Quaternion.AngleAxis(Random.value * 360, Vector3.right); // random rotation
    }

    void TestVictory(int player1CubeGrabed, int player2CubeGrabed)
    {
        if (player1CubeGrabed + player2CubeGrabed != totalCubeCount) return;

        Time.timeScale = 0;
        Debug.Log("Win");
    }
}
