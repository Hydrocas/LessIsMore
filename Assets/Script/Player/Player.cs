using Com.IsartDigital.Common.Utils.Game.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.DontLetThemFall.Player {
	public class Player : MonoBehaviour {
		[Header("Start")]
		[SerializeField] protected float orientationStart = 90;
		[SerializeField] protected float radius = 10;
		[SerializeField] protected Transform centreLevel = default;

		[Header("Other")]
		[SerializeField] protected string nameAxis = "HorizontalPlayer1";
		[SerializeField] protected GameObject asset = default;
		[SerializeField] protected float speed = 10;
		[SerializeField] protected Trigger3DInChild triggerInChild;

		


		protected Action doAction;

		void Start() {
			SetModeMove();

			transform.position = centreLevel.position;
			asset.transform.localPosition = new Vector3(0, 0, radius);
			transform.Rotate(Vector3.up, orientationStart);

			//triggerInChild.OnTrigger3DEnter
		}

		void Update() {
			doAction();
		}

		//Move
		protected void Move() {
			transform.Rotate(Vector3.up, speed * Input.GetAxis(nameAxis) * Time.deltaTime);
		}

		//DoAction
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
