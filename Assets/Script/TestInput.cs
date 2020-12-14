using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    private void OnMove(InputValue inputValue)
    {
        Debug.Log(inputValue);
    }
}
