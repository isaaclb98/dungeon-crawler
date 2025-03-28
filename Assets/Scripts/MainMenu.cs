using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenuMusic");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        MusicManager.Instance.PlayMusic("AmbienceSound");
    }
}
