using Com.IsartDigital.DontLetThemFall.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
	[SerializeField] private AudioClip hitSound = null;
	[SerializeField] private GameObject collisionParticle = null;
	[SerializeField] private string collisionTag = "Ground";
	[Space]
	public Vector2 pitchVariance = new Vector2(0.9f, 1.1f);

	private GameObject particle = null;

	private AudioSource audiosource = null;

	//Hit
	protected Player _playerParent;
	public Player PlayerParent => _playerParent;
	protected Rigidbody rb;
	protected Collider currentCollider;

	private void Awake() {
		audiosource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
		currentCollider = GetComponent<Collider>();
	}

	private void OnCollisionEnter(Collision collision) {
		GameObject lGameObject = collision.gameObject;

		if (lGameObject.CompareTag(collisionTag)) {
			SpawnParticleCollision(lGameObject, collision.GetContact(0).point);
			audiosource.pitch = Random.Range(pitchVariance.x, pitchVariance.y);
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
		GameObject lGameObject = other.gameObject;

		Player lPlayer = lGameObject.GetComponent<Player>();

		if (lPlayer != null) {
			CubeAddOnPlayer(lPlayer);
		}
	}

	//Met les cubes sur le player
	protected void CubeAddOnPlayer(Player player) {
		_playerParent = player;
		transform.parent = player.transform;
		rb.isKinematic = true;
		currentCollider.enabled = false;

		player.AddCubeToScore(this);
	}

	//Enlève le cube  du player
	public void CubeRemoveOnPlayer() {
		_playerParent = null;
		transform.parent = null;
		rb.isKinematic = false;
		currentCollider.enabled = true;
	}
}
