using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public FirstPersonController controller;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        controller.enabled = true; // Re-enable player movement
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; // Hide cursor
        paused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        controller.enabled = false; // Disable player movement
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Show cursor
        paused = true;
    }

    public void Menu()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene("Play menu");
    }

    public void Quit()
    {
        Debug.Log("Quit button pressed!"); // Check if this shows in the Console
        Application.Quit();

        // Ensure this only runs in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
