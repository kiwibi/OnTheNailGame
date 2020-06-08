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
        if (currentS.name != "Menu")
        {
            PlayMusic(1);
        }
        else
        {
            PlayMusic(0);
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

    public void PlayMusic(int song)
    {
        string name = "";

        switch (song)
        {
            case 0:
                name = "MenuTheme";
                break;

            case 1:
                name = "Theme";
                break;
        }

        sounds p = Array.Find(Sounds, sounds => sounds.Name == "MenuTheme");
        p.source.Stop();
        sounds q = Array.Find(Sounds, sounds => sounds.Name == "Theme");
        q.source.Stop();
        sounds s = Array.Find(Sounds, sounds => sounds.Name == name);
        s.source.Play();
    }
}
