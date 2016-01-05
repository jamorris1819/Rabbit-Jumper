using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
    AudioSource source1;
    AudioSource source2;
    AudioSource source3;

    void Start()
    {
        source1 = GetComponents<AudioSource>()[0];
        source2 = GetComponents<AudioSource>()[1];
        source3 = GetComponents<AudioSource>()[2];
        source1.volume = source2.volume = source3.volume = PlayerPrefs.GetFloat("soundVolume");
    }

    public void Play(AudioClip clip)
    {
        if (!source1.isPlaying)
        {
            source1.clip = clip;
            source1.volume = PlayerPrefs.GetFloat("soundVolume");
            source1.PlayOneShot(clip);
        }
        else if (!source2.isPlaying)
        {
            source2.clip = clip;
            source2.volume = PlayerPrefs.GetFloat("soundVolume");
            source2.PlayOneShot(clip);
        }
        else
        {
            source3.clip = clip;
            source3.volume = PlayerPrefs.GetFloat("soundVolume");
            source3.PlayOneShot(clip);
        }
    }
}