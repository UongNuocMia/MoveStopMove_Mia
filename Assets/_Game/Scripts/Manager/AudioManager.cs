using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] public Sound[] musicSound, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private float timeToSwitch;

    private void Start()
    {
        musicSource.loop = true;
    }

    public void PlayMusic(ESound soundName)
    {
        Sound sound = Array.Find(musicSound, x => x.soundType == soundName);

        if (sound == null)
            return;
        musicSource.volume = UserDataManager.Ins.GetMusicVolume();
        musicSource.clip = sound.clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlaySFX(ESound soundName)
    {
        Sound s = Array.Find(sfxSounds, x => x.soundType == soundName);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }
        else
        {
            sfxSource.volume = UserDataManager.Ins.GetSFXVolume();
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
