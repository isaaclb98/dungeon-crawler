using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePortal : MonoBehaviour
{
    // When true, call PlayerStats.Win instead of loading a new scene.
    public bool useWinFunction = false;
    public string sceneToLoad = "Level2";

    // Position to place the player at after teleporting
    private Vector3 targetPosition = new Vector3(0.004f, -1.25f, 0.446f);
    private Vector3 targetRotation = new Vector3(0f, -66.086f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(other.gameObject); // Keep player when loading new scene

            if (useWinFunction)
            {
                PlayerStats.Instance.Win();
            }
            else
            {
                SceneManager.sceneLoaded += (scene, mode) =>
                {
                    other.transform.position = targetPosition;
                    other.transform.eulerAngles = targetRotation;
                };

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}