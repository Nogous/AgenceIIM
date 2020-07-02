using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0f,1f)] public float volumeGainSetting = 0.7f;
    public GameObject MusicPrefab;
    public static float volumeGainGlobal;
    public Sound[] sounds;
    public static AudioManager instance;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError(this + " in " + gameObject.name + " has be destroyed because another " + this + " already exists");
            Destroy(gameObject);
            return;
        }
        volumeGainGlobal = volumeGainSetting;
        foreach (Sound s in sounds)
        {
            if(s.source == null)
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;
            if(s.playOnAwake)
            {
                s.source.playOnAwake = true;
                s.source.Play();
            }
            s.source.volume = volumeGainGlobal;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        MusicPrefab = Resources.Load<GameObject>("Prefab/Musique");
        SpawnMusic();
    }

    public void SpawnMusic()
    {
        if(GameManager.instance != null)
        {
            switch(GameManager.instance.idMonde)
            {
                case Monde.Monde1 :
                {
                    GameObject musicGO = Instantiate(MusicPrefab);
                    musicGO.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("/Audio/Musique/Music_Monde1");
                    musicGO.GetComponent<AudioSource>().volume = volumeGainGlobal;
                    musicGO.GetComponent<AudioSource>().loop = true;
                    DontDestroyOnLoad(musicGO);
                    musicGO.GetComponent<AudioSource>().Play();

                }
                break;
                case Monde.Monde2 :
                {
                    GameObject musicGO = Instantiate(MusicPrefab);
                    musicGO.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("/Audio/Musique/Music_Monde2");
                    musicGO.GetComponent<AudioSource>().volume = volumeGainGlobal;
                    musicGO.GetComponent<AudioSource>().loop = true;
                    DontDestroyOnLoad(musicGO);
                    musicGO.GetComponent<AudioSource>().Play();
                }
                break;
                case Monde.Monde3 :
                {
                    GameObject musicGO = Instantiate(MusicPrefab);
                    musicGO.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("/Audio/Musique/Music_Monde3");
                    musicGO.GetComponent<AudioSource>().volume = volumeGainGlobal;
                    musicGO.GetComponent<AudioSource>().loop = true;
                    DontDestroyOnLoad(musicGO);
                    musicGO.GetComponent<AudioSource>().Play();
                }
                break;
            }
        }
        else
        {
            return;
        }
    }
    public void ChangeGainValue(Single param)
    {
        volumeGainSetting = param;
        volumeGainGlobal = volumeGainSetting;
        foreach (Sound s in sounds)
        {
            s.source.volume = volumeGainGlobal;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    #region Edit Sound

    public void PitchChange(string name, float _pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.pitch = _pitch;
    }

    public void VolumeChange(string name, float _volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.volume = _volume;
    }
    #endregion
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    
    [HideInInspector] public AudioSource source;
    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(.1f, 3f)] public float pitch = 1f;
    public bool playOnAwake;
    public bool loop;
}


