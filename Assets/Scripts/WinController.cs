using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private float timer;
    private bool delay;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.UI.Accept.performed += LoadMainMenu;
    }

    private void LoadMainMenu(InputAction.CallbackContext context)
    {
        if (delay)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Update()
    {
        if (timer > 2) {
            delay = true;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        _inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        _inputActions.UI.Disable();
    }
}
