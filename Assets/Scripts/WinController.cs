using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        FindObjectOfType<MusicManager>().GetComponent<AudioSource>().Stop();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
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
