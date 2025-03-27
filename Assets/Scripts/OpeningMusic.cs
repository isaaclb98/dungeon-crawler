using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource musicSource; // Drag and drop the AudioSource in the Inspector

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.Play(); // Start playing music when the scene starts
        }
    }
}
