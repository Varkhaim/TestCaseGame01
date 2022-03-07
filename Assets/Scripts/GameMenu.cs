using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject GameMenuObject;
    public GameObject GameOverObject;
    public GameObject Controls;
    private bool gamePaused = false;
    private bool isGameOver = false;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (isGameOver) return;
        if (CheckMenuButtonPressed())
        {
            if (gamePaused)
                UnpauseGame();
            else
                PauseGame();
        }
    }

    private bool CheckMenuButtonPressed()
    {
        if (Gamepad.current != null)
            return Gamepad.current.leftTrigger.wasPressedThisFrame;
        return Keyboard.current.escapeKey.wasPressedThisFrame;
    }

    public void ShowGameOverMenu()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        GameOverObject.SetActive(true);
        Controls.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        GameMenuObject.SetActive(true);
        Controls.SetActive(false);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        GameMenuObject.SetActive(false);
        Controls.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
