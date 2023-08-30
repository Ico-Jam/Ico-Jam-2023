using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController[] players;

    [SerializeField]
    private GameObject winCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        players = FindObjectsOfType<PlayerController>();
    }

    public IEnumerator Merge(PlayerController mamushka, Transform position)
    {
        foreach (PlayerController player in players)
        {
            player._inputActions.Disable();
        }
        LeanTween.rotateZ(position.GetChild(0).gameObject, 130, 0.25f);
        yield return new WaitForSeconds(0.25f);
        mamushka.Merge(position);
        yield return new WaitForSeconds(0.25f);
        LeanTween.rotateZ(position.GetChild(0).gameObject, 0, 0.25f);
        yield return new WaitForSeconds(0.25f);
        foreach (PlayerController player in players)
        {
            player._inputActions.Enable();
        }
    }

    public void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }else if(winCanvas != null)
        {
            winCanvas.SetActive(true);
        }

    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
