using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string sceneName = "";

    public void GameStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
