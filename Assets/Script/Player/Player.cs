using Com.IsartDigital.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

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

		protected float forceExterior;
		protected int directionForceExterior;

		protected Action doAction;
		protected Action boingAction;

		public float DirectionAxis {
			get { return Input.GetAxis(nameAxis); }
		}

		public Vector3 AssetPosition {
			get { return asset.transform.position; }
		}

		void Start() {
			SetModeMove();

			boingAction = DoActionVoid;

			transform.position = centreLevel.position;
			myCollider.center = new Vector3(0, 0, -radiusLevel);
			asset.transform.localPosition = new Vector3(0, 0, -radiusLevel);
			
			transform.Rotate(Vector3.up, orientationStart);
		}

		void Update() {
			doAction();
		}

		//Move
		protected void Move() {
			float lDeltaTime = Time.deltaTime;

			transform.Rotate(Vector3.up, speed * DirectionAxis * lDeltaTime);
			asset.transform.localRotation = Quaternion.Euler(0, 0, DirectionAxis * tiltAnglePower); // Tilt Rotation

			boingAction();
		}

		//Collision
		protected void OnTriggerEnter(Collider collision) {
			if (collision.CompareTag(tagPlayer)) {
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
			if (otherPlayer.DirectionAxis == 0 || DirectionAxis == 0) {
				forceExterior = powerForceExterior;
			}
			else if (Mathf.Sign(otherPlayer.DirectionAxis) != Mathf.Sign(DirectionAxis)) {
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
		}


		//Gestion des cubes
		List<Cube> cubesInPlayer = new List<Cube>();

		//Pour le score savoir quelle cube sont dans le player
		public void AddCubeToScore(Cube cube) {
			cubesInPlayer.Add(cube);
		}

		public void RemoveAllCubes() {
			for (int i = cubesInPlayer.Count - 1; i >= 0; i--) {
				cubesInPlayer[i].CubeRemoveOnPlayer();
				cubesInPlayer.RemoveAt(i);
			}
		}
	}
}
