using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public enum SoundInfoName
    {
        wordNotGuessed,
        levelBegin,
        levelEnd,
        letterButtonClicked,
        progressBarStep
    }

    [System.Serializable]
    public class SoundInfo
    {
        public SoundInfoName name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        
        [HideInInspector] public AudioSource source;
    }

    [SerializeField] private List<SoundInfo> soundsList = new List<SoundInfo>(); 
    private Dictionary<SoundInfoName, SoundInfo> soundDictionary = new Dictionary<SoundInfoName, SoundInfo>();

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SoundManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    _instance = obj.AddComponent<SoundManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSounds();
    }

    private void InitializeSounds()
    {
        foreach (SoundInfo sound in soundsList)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            
            soundDictionary.Add(sound.name, sound);
        }
    }

    public void Play(SoundInfoName soundName, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundInfo sound))
        {
            sound.source.pitch = pitch;
            sound.source.Play();
            
        }
        else
        {
            UnityEngine.Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    public void Stop(SoundInfoName soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundInfo sound))
        {
            sound.source.Stop();
        }
    }

    public void SetVolume(SoundInfoName soundName, float volume)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundInfo sound))
        {
            sound.source.volume = Mathf.Clamp(volume, 0f, 1f);
        }
    }

    public void SetPitch(SoundInfoName soundName, float pitch)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundInfo sound))
        {
            sound.source.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        }
    }

    public void ToggleMuteAll(bool mute)
    {
        AudioListener.volume = mute ? 0 : 1;
    }
}