using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    private void OnLevelWasLoaded(int level)
    {
        DontDestroyOnLoad(this);
        if (level == 0) {
            Destroy(gameObject);
        }
    }
}
