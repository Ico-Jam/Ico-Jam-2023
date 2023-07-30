using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    private PlayerInputActions uiInputActions;

    private void Awake()
    {
        uiInputActions = new PlayerInputActions();
        uiInputActions.UI.Escape.performed += CloseSettings;

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        SetMasterVolume(masterVolume);
        SetMusicVolume(masterVolume);
        SetSFXVolume(masterVolume);
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        if (Time.timeSinceLevelLoad <= 0) gameObject.SetActive(false);
    }

    private void CloseSettings(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(false);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void OnEnable()
    {
        uiInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        uiInputActions.UI.Disable();
    }
}
