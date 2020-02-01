using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public sounds[] Sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(sounds s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Start()
    {
        Scene currentS = SceneManager.GetActiveScene();
        if(currentS.name != "Menu")
        {
           Play("Theme");
        }
        else
        {
            Play("MenuTheme");
        }
       
    }

    public void Play (string name)
    {
       sounds s = Array.Find(Sounds, sounds => sounds.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
           
        s.source.Play();
    }
}
