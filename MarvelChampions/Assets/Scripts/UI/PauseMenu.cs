using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    private bool pauseMenuOpen = false;

    public static event UnityAction OnRestartGame;  

    private void Awake()
    {
        pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenuOpen);
            pauseMenuOpen = !pauseMenuOpen;
        }
    }

    public void Restart()
    {
        OnRestartGame?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Debug.Log("Returning to Menu");
        //SceneManager.LoadScene("Menu");
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        pauseMenuOpen = false;
    }
}
