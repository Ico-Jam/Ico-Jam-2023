using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _quitButton;

    [SerializeField]
    private Slider _masterSlider, _musicSlider, _sfxSlider;

    [SerializeField]
    private SettingsMenu _settingsMenu;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _quitButton.SetActive(false);
        }

        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        _settingsMenu.SetMasterVolume(masterVolume);
        _settingsMenu.SetMusicVolume(masterVolume);
        _settingsMenu.SetSFXVolume(masterVolume);
        _masterSlider.value = masterVolume;
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
