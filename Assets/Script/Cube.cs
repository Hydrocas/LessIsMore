using Com.IsartDigital.DontLetThemFall.Player;
using System;
using UnityEngine;

public class Cube : MonoBehaviour {
	[SerializeField] private AudioClip hitSound = null;
	[SerializeField] private GameObject collisionParticle = null;
	[SerializeField] private GameObject collectParticle = null;
	[SerializeField] private string collisionTag = "Ground";
	[Space]
	[SerializeField] protected Vector2 forceExpulsion = new Vector2(800, 1000);

	public Vector2 pitchVariance = new Vector2(0.9f, 1.1f);

	private GameObject particle = null;

	private AudioSource audiosource = null;

	protected bool isOnCube = false;

	//Hit
	protected Player _playerParent;
	public Player PlayerParent => _playerParent;
	protected Rigidbody rb;
	protected Collider currentCollider;

	protected Action doAction;
	protected float elaspedTime = 0;
	[SerializeField] protected float timeMaxToWait = 2;

	private void Awake() {
		audiosource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		currentCollider = GetComponent<Collider>();
	}

	protected void Start() {
		SetModeVoid();
	}

	protected void Update() {
		doAction();
	}

	private void OnCollisionEnter(Collision collision) {
		GameObject lGameObject = collision.gameObject;

		if (lGameObject.CompareTag(collisionTag)) {
			SpawnParticleCollision(lGameObject, collision.GetContact(0).point);
			audiosource.pitch = UnityEngine.Random.Range(pitchVariance.x, pitchVariance.y);
			audiosource.PlayOneShot(hitSound);
		}
	}

	private void SpawnParticleCollision(GameObject other, Vector3 contactPosition) {
		//Vector3 lookDirection = Vector3.Cross(contactPosition - transform.position, transform.right); OLD
		Vector3 lookDirection = transform.position - contactPosition;
		//lookDirection.y = 0;

		particle = Instantiate(collisionParticle);
		particle.transform.position = contactPosition;
		particle.transform.rotation = Quaternion.LookRotation(lookDirection);
		//particle.transform.rotation = Quaternion.AngleAxis(-90, particle.transform.forward) * particle.transform.rotation;

		Destroy(particle, 0.6f);
	}

	protected void OnTriggerEnter(Collider other) {
		if (isOnCube) return;

		GameObject lGameObject = other.gameObject;

		Player lPlayer = lGameObject.GetComponent<Player>();

		if (lPlayer != null) {
			CubeAddOnPlayer(lPlayer);
		}
	}

	//Met les cubes sur le player
	protected void CubeAddOnPlayer(Player player) {
		if (player.AddCubeToScore(this)) return;

		_playerParent = player;
		transform.parent = player.transform;
		rb.isKinematic = true;
		currentCollider.enabled = false;

		isOnCube = true;
		Destroy(Instantiate(collectParticle), 0.5f);
	}

	//Enlève le cube  du player
	public void CubeRemoveOnPlayer(Vector3 force) {
		_playerParent = null;
		transform.parent = null;
		rb.isKinematic = false;
		currentCollider.enabled = true;
		SetModeWait();


		//Effet de boom
		force.y += 0.5f;
		rb.AddForce(force * UnityEngine.Random.Range(forceExpulsion.x, forceExpulsion.y));
	}

	//StateMachine
	protected void SetModeVoid() {
		doAction = DoActionVoid;
	}

	protected void DoActionVoid() {}

	protected void SetModeWait() {
		doAction = DoActionWait;
		elaspedTime = 0;
	}

	//Permet d'empêcher que le cube de se remette direct sur le player
	protected void DoActionWait() {
		elaspedTime += Time.deltaTime;
		
		if (elaspedTime >= timeMaxToWait) {
			SetModeVoid();
			isOnCube = false;
			elaspedTime = 0;
		}
	}
}
