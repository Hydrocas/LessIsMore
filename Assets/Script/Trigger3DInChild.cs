///-----------------------------------------------------------------
/// Author : Valérian Segado
/// Date : 30/03/2020 14:56
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Utils.Game.Triggers {
	public delegate void Trigger3DEventHandler(Collider collision);

	[RequireComponent(typeof(Collider))]
	public class Trigger3DInChild : MonoBehaviour {
		public event Trigger3DEventHandler OnTrigger3DEnter;
		public event Trigger3DEventHandler OnTrigger3DStay;
		public event Trigger3DEventHandler OnTrigger3DExit;

		protected Collider _currentCollider;
		public Collider CurrentCollider => _currentCollider;

		protected void Awake() {
			_currentCollider = GetComponent<Collider>();
		}

		protected void OnTriggerEnter(Collider collision) {
			OnTrigger3DEnter?.Invoke(collision);
		}

		protected void OnTriggerStay(Collider collision) {
			OnTrigger3DStay?.Invoke(collision);
		}

		protected void OnTriggerExit(Collider collision) {
			OnTrigger3DExit?.Invoke(collision);
		}
	}
}