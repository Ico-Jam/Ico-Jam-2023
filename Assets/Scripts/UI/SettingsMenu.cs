using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    private PlayerInputActions _uiInputActions;

    [SerializeField]
    private Slider _masterSlider, _musicSlider, _sfxSlider;

    private void Awake()
    {
        _uiInputActions = new PlayerInputActions();
        _uiInputActions.UI.Escape.performed += CloseSettings;

        Application.targetFrameRate = 60;


        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        _masterSlider.value = masterVolume;
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;
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
