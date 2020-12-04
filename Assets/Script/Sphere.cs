///-----------------------------------------------------------------
/// Author : #DEVELOPER_NAME#
/// Date : #DATE#
///-----------------------------------------------------------------

using Com.IsartDigital.DontLetThemFall.Player;
using UnityEngine;

public class Sphere : MonoBehaviour {

	protected void OnTriggerEnter(Collider other) {
		Player lPlayer = other.gameObject.GetComponent<Player>();

		if (lPlayer != null) {
			lPlayer.RemoveAllCubes(transform.position);
		}
	}
}