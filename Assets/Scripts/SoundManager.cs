using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx3DSource;
    private Dictionary<string, AudioSource> activeLoopingSounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName)
    {
        sfx3DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    public void PlayLoopingSound(string soundName, Vector3 position)
    {
        if (activeLoopingSounds.ContainsKey(soundName)) return; // Already playing

        AudioClip clip = sfxLibrary.GetClipFromName(soundName);
        if (clip != null)
        {
            GameObject soundObj = new GameObject("LoopingSound_" + soundName);
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = true;
            source.spatialBlend = 1.0f;
            source.transform.position = position;
            source.Play();
            activeLoopingSounds[soundName] = source;
        }
    }

    public void StopLoopingSound(string soundName)
    {
        if (activeLoopingSounds.ContainsKey(soundName))
        {
            AudioSource source = activeLoopingSounds[soundName];
            source.Stop();
            Destroy(source.gameObject);
            activeLoopingSounds.Remove(soundName);
        }
    }
}