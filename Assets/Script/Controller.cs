using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {
	public event Action<float> OnMoving;
	public bool useOnMove1 = true;

	private void OnMove1(InputValue inputValue) {
		OnMoving?.Invoke(inputValue.Get<float>());
	}

	protected void OnMove2(InputValue inputValue) {
		OnMoving?.Invoke(inputValue.Get<float>());
	}
}
