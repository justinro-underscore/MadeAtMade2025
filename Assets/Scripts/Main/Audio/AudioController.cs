using System;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffectKeys
{
}

public static class MusicKeys
{
}

[Serializable]
public class GameAudioClip
{
    public AudioClip audioClip;
    [Range(0, 2)]
    public float volume = 1;
    public string audioName = "";
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private GameAudioSource gameAudioSourcePrefab;

    [SerializeField]
    [Range(0, 1)]
    private float baseVolume;

    [SerializeField] private List<GameAudioClip> audioClipsList;
    [SerializeField] private List<GameAudioClip> musicClipsList;

    [SerializeField] private Dictionary<string, GameAudioClip> audioClips;
    [SerializeField] private Dictionary<string, GameAudioClip> musicClips;

    private GameAudioSource musicSource;
    private Dictionary<int, GameAudioSource> oneShotAudioSources = new Dictionary<int, GameAudioSource>();
    private int oneShotAudioSourceNextId = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioClips = new Dictionary<string, GameAudioClip>(audioClipsList.Count);
        foreach (GameAudioClip gameClip in audioClipsList)
        {
            audioClips[gameClip.audioName != "" ? gameClip.audioName : gameClip.audioClip.name] = gameClip;
        }
        musicClips = new Dictionary<string, GameAudioClip>(musicClipsList.Count);
        foreach (GameAudioClip gameClip in musicClipsList)
        {
            musicClips[gameClip.audioName != "" ? gameClip.audioName : gameClip.audioClip.name] = gameClip;
        }

        VerifyAudioSources();

        musicSource = Instantiate(gameAudioSourcePrefab, transform);
        musicSource.gameObject.name = "MusicSouce"; // Not necessary, but helpful!
        musicSource.SetLooping(true);
    }

    private void Update()
    {
        List<int> audioClipIds = new List<int>(oneShotAudioSources.Keys);
        foreach (int audioClipId in audioClipIds)
        {
            GameAudioSource audioSource = oneShotAudioSources[audioClipId];
            if (!audioSource.IsPlaying())
            {
                Destroy(audioSource.gameObject);
                oneShotAudioSources.Remove(audioClipId);
            }
        }
    }

    // TODO replace optional params with options struct
    public int PlayOneShotAudio(string key, bool looping = false, float volume = -1f)
    {
        if (audioClips == null) return -1;
        if (audioClips.ContainsKey(key))
        {
            GameAudioSource audioSource = Instantiate(gameAudioSourcePrefab, transform);
            audioSource.SetAudioClip(audioClips[key].audioClip);
            audioSource.SetVolume((volume >= 0 ? volume : audioClips[key].volume) * baseVolume);
            audioSource.SetLooping(looping);
            oneShotAudioSources.Add(oneShotAudioSourceNextId, audioSource);
            return oneShotAudioSourceNextId++;
        }
        else
        {
            throw new Exception(String.Format("Audio clip not available {0}", key));
        }
    }

    public bool OneShotAudioPlaying(string key)
    {
        foreach (GameAudioSource audioSource in oneShotAudioSources.Values)
        {
            // This doesn't actually work unless the clip name is the same as the key name
            if (audioSource.GetClipName() == key) return true;
        }
        return false;
    }

    public void StopOneShotAudio(int audioId)
    {
        if (oneShotAudioSources.ContainsKey(audioId))
        {
            oneShotAudioSources[audioId].Stop();
        }
    }

    public bool IsOneShotAudioPlaying(int audioId)
    {
        return oneShotAudioSources.ContainsKey(audioId) && oneShotAudioSources[audioId] && oneShotAudioSources[audioId].IsPlaying();
    }

    public void StopAllOneShotAudio()
    {
        foreach (GameAudioSource audioSource in oneShotAudioSources.Values)
        {
            audioSource.Stop();
            // Audio source will be cleaned up in the next Update
        }
    }

    public void PlayMusic(string key, float volume = -1f)
    {
        if (musicClips == null) return;
        if (musicClips.ContainsKey(key))
        {
            musicSource.SetAudioClip(musicClips[key].audioClip);
            musicSource.SetVolume((volume >= 0 ? volume : musicClips[key].volume) * baseVolume);
        }
        else
        {
            throw new Exception(String.Format("Music clip not available {0}", key));
        }
    }

    public float AdjustMusicVolume(float volumeDelta)
    {
        float newVolume = Mathf.Clamp01(musicSource.GetVolume() + volumeDelta);
        musicSource.SetVolume(newVolume);
        return newVolume;
    }

    public bool IsMusicPlaying()
    {
        return musicSource.IsPlaying();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    /**************************************************/

    public void VerifyAudioSources()
    {
        List<string> invalidSoundEffectKeys = new List<string>();
        foreach (System.Reflection.FieldInfo constant in typeof(SoundEffectKeys).GetFields())
        {
            string clipName = (string)constant.GetValue(null);
            if (!audioClips.ContainsKey(clipName))
            {
                invalidSoundEffectKeys.Add(clipName);
            }
        }
        
        List<string> invalidMusicKeys = new List<string>();
        foreach (System.Reflection.FieldInfo constant in typeof(MusicKeys).GetFields())
        {
            string clipName = (string)constant.GetValue(null);
            if (!musicClips.ContainsKey(clipName))
            {
                invalidMusicKeys.Add(clipName);
            }
        }

        if (invalidSoundEffectKeys.Count > 0 || invalidMusicKeys.Count > 0)
        {
            string errorStr = "The following audio clips do not exist!";
            if (invalidSoundEffectKeys.Count > 0)
            {
                errorStr += "\nSound effects:";
                foreach (string clipName in invalidSoundEffectKeys)
                {
                    errorStr += String.Format("\n - {0}", clipName);
                }
            }
            if (invalidMusicKeys.Count > 0)
            {
                errorStr += "\nMusic:";
                foreach (string clipName in invalidMusicKeys)
                {
                    errorStr += String.Format("\n - {0}", clipName);
                }
            }
            throw new Exception(errorStr);
        }
    }
}
