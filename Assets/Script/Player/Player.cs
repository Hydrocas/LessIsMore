using System;
using UnityEngine;

namespace Com.IsartDigital.DontLetThemFall.Player {
	public class Player : MonoBehaviour {
		[Header("Start")]
		[SerializeField] protected float orientationStart = 90;
		[SerializeField] protected float radiusLevel = 10;
		[SerializeField] protected Transform centreLevel = default;

		[Header("Movement")]
		[SerializeField] protected string nameAxis = "HorizontalPlayer1";
		[SerializeField] protected GameObject asset = default;
		[SerializeField] protected BoxCollider myCollider = default;
		[SerializeField] protected float speed = 10;

		[Header("Collision")]
		[SerializeField] protected string tagPlayer = "Player";
		[SerializeField] protected float decreaseForceExterior = 1;
		[SerializeField] protected float powerForceExterior = 30;

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
			

			boingAction();
		}

		//Collision
		protected void OnTriggerEnter(Collider collision) {
			if (collision.CompareTag(tagPlayer)) {
				Player lPlayer = collision.GetComponent<Player>();
				Boing(lPlayer);
			}
		}

		protected void Boing(Player otherPlayer) {
			/*if (Mathf.Sign(otherPlayer.DirectionAxis) != Mathf.Sign(DirectionAxis)) {
				forceExterior = 300;
			}
			else {
				forceExterior = 300;
			}*/

			Vector3 lDirection = otherPlayer.AssetPosition - AssetPosition;

			if (Vector3.Dot(lDirection, asset.transform.right) < 0) {
				forceExterior = powerForceExterior;
				directionForceExterior = -1;
				boingAction = BoingActionAddForceExterior;
			}
			else {
				forceExterior = powerForceExterior;
				directionForceExterior = 1;
				boingAction = BoingActionAddForceExterior;
			}
		}

		//DoAction
		protected void BoingActionAddForceExterior() {
			transform.Rotate(Vector3.up, forceExterior * Time.deltaTime * directionForceExterior);
			forceExterior -= decreaseForceExterior;

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
	}
}
