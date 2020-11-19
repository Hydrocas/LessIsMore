using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.DontLetThemFall.Player {
	public class Player : MonoBehaviour {
		[SerializeField] protected string nameAxis = "HorizontalPlayer1";

		protected Action doAction;

		void Start() {
			SetModeVoid();
		}

		void Update() {
			doAction();
		}

		protected void SetModeVoid() {
			doAction = DoActionVoid;
		}

		protected void DoActionVoid() { }
	}
}
