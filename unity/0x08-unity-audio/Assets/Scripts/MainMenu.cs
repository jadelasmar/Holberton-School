﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    public void LevelSelect(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Options()
    {
        // Save current scene to PlayerPrefs for back reference.
        PlayerPrefs.SetInt(PlayerPrefKeys.previousScene, SceneManager.GetActiveScene().buildIndex);

        // Load Options menu.
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}