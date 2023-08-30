using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    private PlayerInputActions _uiInputActions;

    private void Awake()
    {
        _uiInputActions = new PlayerInputActions();
        _uiInputActions.UI.Escape.performed += CloseSettings;

        Application.targetFrameRate = 60;
    }

    private void CloseSettings(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(false);
    }

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void OnEnable()
    {
        _uiInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        _uiInputActions.UI.Disable();
    }
}
