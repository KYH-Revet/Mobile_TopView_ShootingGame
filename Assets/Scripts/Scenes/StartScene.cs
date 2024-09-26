using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string sceneName = "";

    public void GameStart()
    {
        // Time Scale always 1
        Time.timeScale = 1f;

        // Load Scene
        SceneManager.LoadScene(sceneName);
    }
}
