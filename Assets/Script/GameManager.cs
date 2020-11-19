using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int cubeCount = 10;
    public float spawnFrequency = 1;
    public int spawnNumber = 3;
    public Transform spawnPoint = null;
    public Transform spawnPointRadius = null;

    public GameObject cubePrefab = null;

    private float deltaTime = 0;
    private Vector3 radiusSpawn;

    private void Start()
    {
        radiusSpawn = spawnPointRadius.position - spawnPoint.position;
        StartCoroutine(SpawnCubeRoutine());
    }

    IEnumerator SpawnCubeRoutine()
    {
        while (true)
        {
            cubeCount -= spawnNumber;

            int numCubeToSpawn = cubeCount < 0 ? spawnNumber + cubeCount : spawnNumber;

            for (int i = numCubeToSpawn - 1; i >= 0; i--)
            {
                SpawnCube(Instantiate(cubePrefab));
            }

            if (numCubeToSpawn < 0) break;

            yield return new WaitForSeconds(spawnFrequency);
        }

        Debug.Log("all cube spawned");
    }

    void SpawnCube(GameObject cube)
    {
        Vector3 radiusPos = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * radiusSpawn;
        cube.transform.position = spawnPoint.position + radiusPos; // random position
        cube.transform.rotation = Quaternion.AngleAxis(Random.value * 360, Vector3.right); // random rotation
    }
}
