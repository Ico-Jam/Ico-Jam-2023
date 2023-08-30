using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    [SerializeField]
    private GameObject _menuContainer, _settingsMenu;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.UI.Escape.performed += PauseInput;
    }

    private void PauseInput(InputAction.CallbackContext context)
    {
        ToggleMenu();
    }

    public void ToggleMenu()
    {
        Time.timeScale = _menuContainer.activeSelf ? 1 : 0;
        _menuContainer.SetActive(!_menuContainer.activeSelf);
    }

    public void OpenSettings()
    {
        _settingsMenu.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
