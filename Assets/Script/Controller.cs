using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {
	public event Action<float> OnMoving;

	private void OnMove(InputValue inputValue) {
		OnMoving?.Invoke(inputValue.Get<float>());
	}
}
