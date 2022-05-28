using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void VolumeLevel (float sliderValue)
    {
        audioMixer.SetFloat("Vol", Mathf.Log10 (sliderValue)*20);
    }
}
