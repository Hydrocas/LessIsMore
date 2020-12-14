using Com.IsartDigital.Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.IsartDigital.DontLetThemFall.Player {
	public class Player : MonoBehaviour {

		[Header("Start")]
		[SerializeField] protected float orientationStart = 90;
		[SerializeField] protected float radiusLevel = 10;
		[SerializeField] protected Transform centreLevel = default;

		[Header("Movement")]
		[SerializeField] protected string nameAxis = "HorizontalPlayer1";
		[SerializeField] public GameObject asset = default;
		[SerializeField] protected BoxCollider myCollider = default;
		[SerializeField] protected float speed = 10;
		[SerializeField] protected float tiltAnglePower = 1;

		[Header("Collision")]
		[SerializeField] protected string tagPlayer = "Player";
		[SerializeField] protected float decreaseForceExterior = 1;
		[SerializeField] protected float powerForceExterior = 30;
		[SerializeField, Range(1, 2)] protected float powerMultiply = 1;
		[SerializeField] protected GameObject collisionParticle = null;

		[Header("Shake")]
		[SerializeField] protected CameraEffect cameraEffect = null;
		[SerializeField] protected float cameraEffectDuration = 1;
		[SerializeField] protected float cameraEffectStrengh = 1;

        [Header("SFX")]
        [SerializeField] protected List<AudioClip> punchSounds = new List<AudioClip>();

		[Header("Stun + Collision with sphere")]
		[SerializeField] protected float timeMaxToStun = 2;
		//Rotation quand il touche un sphere
		[SerializeField] protected float forceToStun = 50;

		protected float forceExterior;
		protected int directionForceExterior;

		protected Action doAction;
		protected Action boingAction;

		//collisionToSphere
		protected List<Cube> cubesInPlayer = new List<Cube>();
		protected bool isStun = false;
		protected float elapsedTime = 0;
		protected Rigidbody rbAsset;

        protected AudioSource audioSource;
		private Controller controller;

		private float directionAxis;

		public Vector3 AssetPosition {
			get { return asset.transform.position; }
		}

		public void ListenController(Controller controller)
		{
			this.controller = controller;
			controller.OnMoving += Controller_OnMoving;
		}

		private void Controller_OnMoving(float value)
		{
			directionAxis = value;
		}

		protected void Start() {
			SetModeMove();

			boingAction = DoActionVoid;

			transform.position = centreLevel.position;
			myCollider.center = new Vector3(0, 0, -radiusLevel);
			asset.transform.localPosition = new Vector3(0, 0, -radiusLevel);
			
			transform.Rotate(Vector3.up, orientationStart);

            audioSource = GetComponent<AudioSource>();

			rbAsset = asset.GetComponent<Rigidbody>();
		}

		protected void Update() {
			doAction();
		}

		//Move
		protected void Move() {
			float lDeltaTime = Time.deltaTime;

			transform.Rotate(Vector3.up, speed * directionAxis * lDeltaTime);

			boingAction();
		}
		
		protected void Tilt() {
			asset.transform.localRotation = Quaternion.Euler(0, 0, directionAxis * tiltAnglePower); // Tilt Rotation
		}

		//Collision
		protected void OnTriggerEnter(Collider collision) {
			if (collision.CompareTag(tagPlayer)) {

				Player lPlayer = collision.GetComponent<Player>();
				Boing(lPlayer);

                int lRand = UnityEngine.Random.Range(0, punchSounds.Count);

                audioSource.PlayOneShot(punchSounds[lRand]);

				Player otherPlayer = collision.GetComponent<Player>();

				BoingEffect(asset.transform.position + (otherPlayer.asset.transform.position - asset.transform.position) / 2);

				Boing(otherPlayer);
			}
		}

		protected void BoingEffect(Vector3 position)
        {
			Transform particleTransform = Instantiate(collisionParticle).transform;
			particleTransform.position = position;
			particleTransform.LookAt(asset.transform);
			Destroy(particleTransform.gameObject, 2f);
			cameraEffect.DoScreenShake(cameraEffectDuration, cameraEffectStrengh);
		}

		protected void Boing(Player otherPlayer) {
			if (otherPlayer.directionAxis == 0 || directionAxis == 0) {
				forceExterior = powerForceExterior;
			}
			else if (Mathf.Sign(otherPlayer.directionAxis) != Mathf.Sign(directionAxis)) {
				forceExterior = powerForceExterior * powerMultiply;
			}
			else {
				forceExterior = powerForceExterior;
			}

			//Bug collision dans master

			Vector3 lDirection = otherPlayer.AssetPosition - AssetPosition;

			if (Vector3.Dot(lDirection, asset.transform.right) < 0) {
				directionForceExterior = -1;
				boingAction = BoingActionAddForceExterior;
			}
			else {
				directionForceExterior = 1;
				boingAction = BoingActionAddForceExterior;
			}
		}

		//DoAction
		protected void BoingActionAddForceExterior() {
			float lDeltaTime = Time.deltaTime;

			transform.Rotate(Vector3.up, forceExterior * lDeltaTime * directionForceExterior);
			forceExterior -= decreaseForceExterior * lDeltaTime;

			//Debug.Log(forceExterior);

			if (forceExterior <= 0) {
				boingAction = DoActionVoid;
			}
		}

		protected void SetModeVoid() {
			doAction = DoActionVoid;
		}

		protected void DoActionVoid() { }

		protected void SetModeMove() {
			doAction = DoActionMove;
		}

		protected void DoActionMove() {
			Move();
			Tilt();
		}

		protected void SetModeWait() {
			doAction = DoActionWait;
			
		}

		protected void DoActionWait() {
			elapsedTime += Time.deltaTime;

			Move();

			if (elapsedTime >= timeMaxToStun) {
				elapsedTime = 0;
				SetModeMove();
				isStun = false;

				asset.transform.rotation = Quaternion.identity;
				rbAsset.angularVelocity = Vector3.zero;
			}
		}


		//Gestion des cubes

		//Pour le score savoir quelle cube sont dans le player
		public bool AddCubeToScore(Cube cube) {
			if (isStun) return true;

			cubesInPlayer.Add(cube);

			return false;
		}

		public void RemoveAllCubes(Vector3 positionSphere) {
			if (isStun) return;

			isStun = true;

			SetModeWait();

			positionSphere.y = asset.transform.position.y;
			Vector3 lDirection = positionSphere - asset.transform.position;

			Vector3.Cross(lDirection, asset.transform.up);

			rbAsset.AddTorque(Vector3.Cross(lDirection, -asset.transform.up) * forceToStun);

			for (int i = cubesInPlayer.Count - 1; i >= 0; i--) {
				cubesInPlayer[i].CubeRemoveOnPlayer(lDirection);
				cubesInPlayer.RemoveAt(i);
			}
		}
	}
}
