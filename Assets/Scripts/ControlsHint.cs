using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsHint : MonoBehaviour
{
    public GameObject KeyboardControls;
    public GameObject GamepadControls;

    private void Update()
    {
        bool isGamepadConnected = Gamepad.current != null;
        GamepadControls.SetActive(isGamepadConnected);
        KeyboardControls.SetActive(!isGamepadConnected);
    }
}
