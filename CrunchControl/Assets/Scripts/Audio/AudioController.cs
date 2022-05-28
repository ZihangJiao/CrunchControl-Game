using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioController : MonoBehaviour
{
    public AudioMixerGroup audioMixerMusic;
    public AudioMixerGroup audioMixerSoundFX;

    public Sound[] sounds;

    void Awake()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.audioMixerType == 1)
            {
                s.source.outputAudioMixerGroup = audioMixerMusic;
            }
            else if (s.audioMixerType == 2)
            {
                s.source.outputAudioMixerGroup = audioMixerSoundFX;
            }
        }
    }

    //To use below: FindObjectOfType<AudioController>().PlaySound("NAMEOFSOUND");

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
}
