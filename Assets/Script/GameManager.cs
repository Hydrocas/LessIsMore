using Com.IsartDigital.Common;
using Com.IsartDigital.DontLetThemFall.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[Header("Player")]
	[SerializeField] protected List<Player> players = default;

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
	[Space]
	public Vector2 pitchVariance = new Vector2(0.9f, 1.1f);

	private Vector3 radiusSpawn;
	private AudioSource audioSource;

	[Header("Win")]
	[SerializeField] protected Canvas hud;
	[SerializeField] protected Canvas splitScreen;
	[SerializeField] protected WinScreen winScreen;

	private void Start() {
		Time.timeScale = 1;
		
		audioSource = GetComponent<AudioSource>();
		radiusSpawn = spawnPointRadius.position - spawnPoint.position;

		StartCoroutine(SpawnLevelObjectRoutine(cubeCount, spawnCubeNumber, spawnCubeFrequency, cubePrefab)); // Cube spawn
		StartCoroutine(SpawnLevelObjectRoutine(sphereCount, spawnSphereNumber, spawnSphereFrequency, spherePrefab)); // Sphere spawn

		reSpawnArea.OnCollisionTrigger += ReSpawnArea_OnCollisionTrigger;

		for (int i = players.Count - 1; i >= 0; i--) {
			players[i].OnWin += Player_OnWin;
		}

		//UI
		winScreen.gameObject.SetActive(false);
		hud.gameObject.SetActive(true);
		splitScreen.gameObject.SetActive(true);
	}

	private void OnDestroy() {
		for (int i = players.Count - 1; i >= 0; i--) {
			players[i].OnWin -= Player_OnWin;
		}
	}

	private void Update() {
		RenderSettings.skybox.SetFloat("_Rotation", Time.time);

		if (Input.GetMouseButtonDown(2)) {
			Win();
		}
	}

	private void ReSpawnArea_OnCollisionTrigger(Collider other) {
		SpawnLevelObject(other.gameObject);
	}

	IEnumerator SpawnLevelObjectRoutine(int spawnTotalNumber, int spawnNumber, float spawnFrequency, GameObject spawnedPrefab) {
		int numToSpawn = spawnNumber;

		while (true) {
			spawnTotalNumber -= spawnNumber;

			if (spawnTotalNumber < 0) {
				numToSpawn = spawnNumber + spawnTotalNumber;
			}

			for (int i = numToSpawn - 1; i >= 0; i--) {
				SpawnLevelObject(Instantiate(spawnedPrefab));
			}

			if (spawnTotalNumber < 0) break;

			yield return new WaitForSeconds(spawnFrequency);
		}

		//Debug.Log("all " + spawnedPrefab + " spawned");
	}

	protected void SpawnLevelObject(GameObject levelObject) {
		Vector3 radiusPos = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * radiusSpawn;
		levelObject.transform.position = spawnPoint.position + radiusPos; // random position
		levelObject.transform.rotation = Quaternion.AngleAxis(Random.value * 360, Vector3.right); // random rotation

		audioSource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
		audioSource.Play();
	}

	/// <summary>
	/// Win
	/// </summary>
	protected void Player_OnWin() {
		int lCubesCount = 0;

		for (int i = players.Count - 1; i >= 0; i--) {
			lCubesCount += players[i].CubesInPlayerCount;
		}

		if (lCubesCount >= cubeCount) {
			Win();
		}
	}

	protected void Win() {
		Time.timeScale = 0;

		hud.gameObject.SetActive(false);

		int leftCount = players[0].CubesInPlayerCount;
		int rightCount = players[1].CubesInPlayerCount;

		if (leftCount > rightCount) winScreen.Init(WinScreen.StateWin.LEFT_WIN);
		else if (leftCount < rightCount) winScreen.Init(WinScreen.StateWin.RIGHT_WIN);
		else winScreen.Init(WinScreen.StateWin.DRAW);
	}
}
