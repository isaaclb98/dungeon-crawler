using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            optionsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
    private void Back()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
